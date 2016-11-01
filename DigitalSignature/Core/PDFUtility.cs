using DigitalSignatureService.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace DigitalSignatureService.Core
{
    public static class PDFUtility
    {
        public static object GetPdfInfo(byte[] pdf)
        {
            using (var reader = new PdfReader(pdf))
            {
                var result = new
                {
                    page_count = reader.NumberOfPages,
                    size = reader.FileLength, 
                    info = reader.Info
                };
                return result;
            }            
        }
        public static object GetPdfInfo(Stream pdf)
        {
            using (var reader = new PdfReader(pdf))
            {
                var result = new
                {
                    page_count = reader.NumberOfPages,
                    size = reader.FileLength,
                    info = reader.Info
                };
                return result;
            }
        }
        public static bool AddPDFFields(string pdfIn, string pdfOut, List<PDFField> pdfFields)
        {
            var bytesIn = File.ReadAllBytes(pdfIn);
            var bytesOut = AddPDFFields(bytesIn, pdfFields);
            File.WriteAllBytes(pdfOut, bytesOut);
            return true;
        }
        public static byte[] AddPDFFields(byte[] bytesIn, List<PDFField> pdfFields)
        {
            var bytesOut = AddPDFTextFields(bytesIn, pdfFields.Where(t => t.type == PDFField.PDFFieldType.TextField).Cast<PdfTextField>());
            bytesOut = AddPdfSignatureFields(bytesOut, pdfFields.Where(t => t.type == PDFField.PDFFieldType.SignatureField).Cast<PdfSignatureField>());
            return bytesOut;
        }
        public static byte[] AddPDFTextFields(byte[] filePdf, IEnumerable<PdfTextField> pdfFields)
        {
            var reader = new PdfReader(filePdf);
            var result = new MemoryStream();
            using (var stamper = new PdfStamper(reader, result))
            {
                foreach (var field in pdfFields)
                {
                    var cb = stamper.GetOverContent(field.page);
                    ColumnText ct = new ColumnText(cb);
                    ct.SetSimpleColumn(new Phrase(new Chunk(field.text, FontFactory.GetFont(field.fontName, field.fontEncoding, field.fontEmbeded, field.fontSize, Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml(field.color))))),
                     field.x, field.y, field.x + field.width, field.y + field.height, field.fontSize, field.align);
                    ct.Go();
                    if (field.showborder)
                    {
                        cb.SetColorStroke(BaseColor.BLUE);
                        cb.Rectangle(field.x, field.y, field.width, field.height);
                        cb.Stroke();
                    }
                }
                stamper.Close();
            }
            reader.Close();
            return result.ToArray();
        }
        public static byte[] AddPdfSignatureFields(byte[] filePdf, IEnumerable<PdfSignatureField> signatureFields)
        {
            byte[] result = filePdf;
            foreach (var field in signatureFields)
            {
                result = AddPdfSignatureField(result, field);
            }
            return result;
        }
        public static byte[] AddPdfSignatureField(byte[] filePdf, PdfSignatureField signatureField)
        {
            var thumbprint = signatureField.thumbprint;
            var page = signatureField.page;
            var x = signatureField.x;
            var y = signatureField.y;
            var location = signatureField.location;
            var width = signatureField.width;
            var height = signatureField.height;
            var reason = signatureField.reason;
            var signatureFieldName = signatureField.name;

            IList<Org.BouncyCastle.X509.X509Certificate> chain;
            X509Certificate2 pk;
            IOcspClient ocspClient;
            ITSAClient tsaClient;
            IList<ICrlClient> crlList;
            CertificateUtility.GetCertificateProperties(thumbprint, out chain, out pk, out ocspClient, out tsaClient, out crlList);


            var result = AddPdfSignatureField(filePdf, chain, pk, DigestAlgorithms.SHA1, CryptoStandard.CMS, reason,
                     location,
                     crlList, ocspClient, tsaClient, 0, page, new Rectangle(x, y, width + x, height + y), signatureFieldName);
            return result;
        }
        private static byte[] AddPdfSignatureField(byte[] src,
                         ICollection<Org.BouncyCastle.X509.X509Certificate> chain, X509Certificate2 pk,
                         string digestAlgorithm, CryptoStandard subfilter,
                         string reason, string location,
                         ICollection<ICrlClient> crlList,
                         IOcspClient ocspClient,
                         ITSAClient tsaClient,
                         int estimatedSize, int page, Rectangle rectangle, string signatureFieldName)
        {
            // Creating the reader and the stamper
            PdfReader reader = null;
            PdfStamper stamper = null;
            var os = new MemoryStream();
            try
            {
                reader = new PdfReader(src);
                stamper = PdfStamper.CreateSignature(reader, os, '\0');
                // Creating the appearance
                var appearance = stamper.SignatureAppearance;
                appearance.Reason = reason;
                appearance.Location = location;
                appearance.SetVisibleSignature(rectangle, page, signatureFieldName);
                // Creating the signature
                IExternalSignature pks = new X509Certificate2Signature(pk, digestAlgorithm);
                MakeSignature.SignDetached(appearance, pks, chain, crlList, ocspClient, tsaClient, estimatedSize,
                                           subfilter);
                return os.ToArray();
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (stamper != null)
                    stamper.Close();
            }
        }

        //public static byte[] AddPDFSignature(byte[] filePdf, List<PDFField> pdfFields)
        //{
        //    var reader = new PdfReader(filePdf);
        //    var result = new MemoryStream();
        //    using (PdfStamper stamper = PdfStamper.CreateSignature(reader, result, '\0'))
        //    {
        //        var KEYSTORE = "";
        //        var PASSWORD = "".ToCharArray();
        //        string alias = "";

        //        Pkcs12Store store = new Pkcs12Store(new FileStream(KEYSTORE, FileMode.Open), PASSWORD);

        //        ICollection<X509Certificate> chain = new List<X509Certificate>();
        //        // searching for private key
        //        foreach (string al in store.Aliases)
        //            if (store.IsKeyEntry(al) && store.GetKey(al).Key.IsPrivate)
        //            {
        //                alias = al;
        //                break;
        //            }
        //        AsymmetricKeyEntry pk = store.GetKey(alias);

        //        foreach (X509CertificateEntry c in store.GetCertificateChain(alias))
        //            chain.Add(c.Certificate);
        //        RsaPrivateCrtKeyParameters parameters = pk.Key as RsaPrivateCrtKeyParameters;


        //        // Creating the appearance
        //        PdfSignatureAppearance appearance = stamper.SignatureAppearance;
        //        appearance.Reason = "My reason for signing";
        //        appearance.Location = "The middle of nowhere";
        //        appearance.SetVisibleSignature(new Rectangle(36, 748, 144, 780), 1, "sig");
        //        // Creating the signature
        //        IExternalSignature pks = new PrivateKeySignature(pk.Key, DigestAlgorithms.SHA256);
        //        MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);
        //    }
        //    return new byte[0];
        //}
    }
}