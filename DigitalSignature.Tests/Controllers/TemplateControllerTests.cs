using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace DigitalSignatureService.Controllers.Tests
{
    [TestClass()]
    public class TemplateControllerTests
    {
        [TestMethod()]
        public void SetExamplePdfTest()
        {
            var con = new TemplateController();

            //var info = con.SetExamplePdf("templateID1", File.ReadAllBytes(@"D:\temp\testSignatureService\BG.pdf"));

            //var content = con.GetExamplePdf("templateID1");
            //var imagebytes = con.RenderPdf("templateID1", 400, 1);
            
            //var img = Image.FromStream(new MemoryStream(imagebytes));
        }
    }
}