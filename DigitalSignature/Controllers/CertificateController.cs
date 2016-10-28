using DigitalSignatureService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DigitalSignatureService.Controllers
{
    public class CertificateController : ApiController
    {
        [HttpGet]
        [ActionName("list")]
        public object GetServerCertificates(string thumbprint = "")
        {
            return CertificateUtility.GetCertificates(thumbprint);
        }
    }
}
