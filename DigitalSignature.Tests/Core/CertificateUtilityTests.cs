using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureService.Core.Tests
{
    [TestClass()]
    public class CertificateUtilityTests
    {
        [TestMethod()]
        public void GetAllCertificatesTest()
        {
            var result = CertificateUtility.GetCertificates();

        }
    }
}