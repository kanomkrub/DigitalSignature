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
using System.Web;

namespace DigitalSignatureService
{
    public class PDFUtility
    {
        public static void AddPDFFields(string pdfIn, string pdfOut, List<PDFField> pdfField)
        {
            var bytesIn = File.ReadAllBytes(pdfIn);
            var bytesOut = AddPDFFields(bytesIn, pdfField);
            File.WriteAllBytes(pdfOut, bytesOut);
        }
        public static byte[] AddPDFFields(byte[] filePdf, List<PDFField> pdfFields)
        {
            var reader = new PdfReader(filePdf);
            var result = new MemoryStream();
            using (PdfStamper stamper = new PdfStamper(reader, result))
            {
                foreach (var field in pdfFields.Where(t => t.type == PDFField.PDFFieldType.TextField))
                {
                    var t = (PdfTextField)field;
                    var cb = stamper.GetOverContent(1);
                    ColumnText ct = new ColumnText(cb);
                    ct.SetSimpleColumn(new Phrase(new Chunk(t.text, FontFactory.GetFont(t.fontName, t.fontEncoding, t.fontEmbeded, t.fontSize, Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml(t.color))))),
                     t.x, t.y, t.x + t.width, t.y + t.height, t.fontSize, t.align);
                    ct.Go();
                    if (t.showborder)
                    {
                        cb.SetColorStroke(BaseColor.BLUE);
                        cb.Rectangle(t.x, t.y, t.width, t.height);
                        cb.Stroke();
                    }
                }
                foreach (var imageField in pdfFields.Where(t => t.type == PDFField.PDFFieldType.ImageField))
                {

                }
                foreach (var signatureField in pdfFields.Where(t => t.type == PDFField.PDFFieldType.SignatureField))
                {

                }
                stamper.Close();
            }
            return result.ToArray();
        }
        public static byte[] AddPDFSignature(byte[] filePdf, List<PDFField> pdfFields)
        {
            var reader = new PdfReader(filePdf);
            var result = new MemoryStream();
            using (PdfStamper stamper = new PdfStamper(reader, result))
            {
                var KEYSTORE = "";
                var PASSWORD = "".ToCharArray();
                string alias = "";

                Pkcs12Store store = new Pkcs12Store(new FileStream(KEYSTORE, FileMode.Open), PASSWORD);

                ICollection<X509Certificate> chain = new List<X509Certificate>();
                // searching for private key
                foreach (string al in store.Aliases)
                    if (store.IsKeyEntry(al) && store.GetKey(al).Key.IsPrivate)
                    {
                        alias = al;
                        break;
                    }
                AsymmetricKeyEntry pk = store.GetKey(alias);

                foreach (X509CertificateEntry c in store.GetCertificateChain(alias))
                    chain.Add(c.Certificate);
                RsaPrivateCrtKeyParameters parameters = pk.Key as RsaPrivateCrtKeyParameters;


                // Creating the appearance
                PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                appearance.Reason = "My reason for signing";
                appearance.Location = "The middle of nowhere";
                appearance.SetVisibleSignature(new Rectangle(36, 748, 144, 780), 1, "sig");
                // Creating the signature
                IExternalSignature pks = new PrivateKeySignature(pk.Key, DigestAlgorithms.SHA256);
                MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);
            }
        }
    }
}