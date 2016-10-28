using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureService.Tests
{
    [TestClass()]
    public class C3_11_SignWithTokenTests
    {
        [TestMethod()]
        public void TestSignTest()
        {
            C3_11_SignWithToken.TestSign(@"D:\temp\testSignatureService\BG.PDF", @"D:\temp\testSignatureService\BG_Signed.PDF");
        }
    }
}