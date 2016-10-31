using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DigitalSignatureService.Controllers.PdfFieldController;

namespace DigitalSignatureService.Controllers.Tests
{
    [TestClass()]
    public class PDFUtilityTests
    {
        [TestMethod()]
        public void AddTemplateFieldsTest()
        {
            var controller = new PdfFieldController();
            var request = new AddFieldsRequest()
            {
                template_id = "5968aa8b-98f5-4793-92b6-cbbfb1ca8b6b",
                path_in = @"D:\temp\testSignatureService\BG.pdf",
                path_out = @"D:\temp\testSignatureService\BG_from_template.pdf"
            };
            controller.AddFields(request);
        }
    }
}