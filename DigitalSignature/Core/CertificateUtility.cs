using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System.Linq;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace DigitalSignatureService.Core
{
    public static class CertificateUtility
    {
        public  static void GetCertificateProperties(string thumbprint, out IList<X509Certificate> chain, out X509Certificate2 pk, out IOcspClient ocspClient, out ITSAClient tsaClient, out IList<ICrlClient> crlList)
        {
            GetPK(thumbprint, out chain, out pk);

            ocspClient = new OcspClientBouncyCastle();
            tsaClient = null;
            for (int i = 0; i < chain.Count; i++)
            {
                X509Certificate cert = chain[i];
                String tsaUrl = CertificateUtil.GetTSAURL(cert);
                if (tsaUrl != null)
                {
                    tsaClient = new TSAClientBouncyCastle(tsaUrl);
                    break;
                }
            }
            crlList = new List<ICrlClient>();
            crlList.Add(new CrlClientOnline(chain));
        }

        private static void GetPK(string thumbprint, out IList<X509Certificate> chain, out X509Certificate2 pk)
        {
            var x509Store = new X509Store( StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);
            var certificates = x509Store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certificates.Count == 0) throw new KeyNotFoundException($"key {thumbprint} not found");
            chain = new List<X509Certificate>();
            pk = certificates[0];
            var x509chain = new X509Chain();
            x509chain.Build(pk);

            foreach (var x509ChainElement in x509chain.ChainElements)
            {
                chain.Add(DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
            }
            x509Store.Close();
        }

        private static List<object> Getlist(X509Certificate2Collection certificates)
        {
            var certificatesList = new List<object>();
            foreach (var certificate in certificates)
            {
                certificatesList.Add(new
                {
                    has_pk = certificate.HasPrivateKey,
                    thumbprint = certificate.Thumbprint,
                    issuer = certificate.Issuer,
                    issuer_name = certificate.IssuerName,
                    not_after = certificate.NotAfter,
                    not_before = certificate.NotBefore,
                    subject = certificate.Subject,
                    subject_name = certificate.SubjectName,
                    version = certificate.Version
                });
            }
            return certificatesList;
        }

        public static List<object> GetCertificates(string thumbprint = "")
        {
            //fix store path => local machine>trusted root certificates
            var x509Store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadOnly);
            var certificates = x509Store.Certificates;
            if (!string.IsNullOrEmpty(thumbprint))
            {
                thumbprint = thumbprint.Replace("‎", "").Replace(" ", "").ToUpper();
                certificates = x509Store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            }
            var certificatesList = Getlist(certificates);
            x509Store.Close();
            return certificatesList;
        }
    }
}
