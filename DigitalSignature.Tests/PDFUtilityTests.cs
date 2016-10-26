using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignatureService.Models;

namespace DigitalSignatureService.Tests
{
    [TestClass()]
    public class PDFUtilityTests
    {
        [TestMethod()]
        public void AddPDFFieldsTest()
        {
            var list = new List<PDFField>();
            list.Add(new PdfTextField()
            {
                text = "HELLOOOOOOOOO",
                x = 150,
                y = 100,
                width = 130,
                height = 200,
                fontSize = 25,
                color = "#FF00008B",
                showborder =true
            });

            PDFUtility.AddPDFFields(@"D:\temp\testSignatureService\BG.pdf", @"D:\temp\testSignatureService\BG_Out.pdf", list);
        }
    }
}