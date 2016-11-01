using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignatureService.Models;
using System.IO;
using DigitalSignatureService.Controllers;
using static DigitalSignatureService.Controllers.PdfFieldController;

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
                showborder = true,
                page = 1
            });
            list.Add(new PdfTextField()
            {
                text = "Test2OOOO",
                x = 210,
                y = 150,
                width = 130,
                height = 200,
                fontSize = 15,
                color = "#FF00008B",
                showborder = true,
                page = 1
            });

            PDFUtility.AddPDFFields(@"D:\temp\testSignatureService\BG.pdf", @"D:\temp\testSignatureService\BG_Out.pdf", list);
        }

        [TestMethod()]
        public void TestSignTest()
        {
            var src = @"D:\temp\testSignatureService\BG.PDF";
            var dest = @"D:\temp\testSignatureService\BG_Signed.PDF";
            var field = new PdfSignatureField()
            {
                name = "signature1",
                x = 100,
                y = 100,
                width = 100,
                height = 100,
                reason = "testJaaa",
                location = "current location",
                page = 1,
                thumbprint = "‎76 61 4a 24 85 76 46 4a 5b 13 75 3a e4 f3 31 4b 7b aa 79 62".Replace("‎", "").Replace(" ", "").ToUpper()
            };
            var result = PDFUtility.AddPdfSignatureField(File.ReadAllBytes(src), field);
            File.WriteAllBytes(dest, result);

            field = new PdfSignatureField()
            {
                name = "signature2",
                x = 200,
                y = 200,
                width = 100,
                height = 100,
                reason = "testJaaa2",
                location = "location",
                page = 1,
                thumbprint = "‎bc 97 b6 69 77 48 9c fb ca a0 78 58 38 19 c5 d6 1f 65 0c b8".Replace("‎", "").Replace(" ", "").ToUpper()
            };

            result = PDFUtility.AddPdfSignatureField(File.ReadAllBytes(dest), field);
            File.WriteAllBytes(dest, result);
        }
        [TestMethod()]
        public void TextWithSignatureTest()
        {
            List<PDFField> list = GetTestList();
            PDFUtility.AddPDFFields(@"D:\temp\testSignatureService\BG.pdf", @"D:\temp\testSignatureService\BG_Out_text_signature.pdf", list);
        }

        public static List<PDFField> GetTestList()
        {
            var list = new List<PDFField>();
            list.Add(new PdfTextField()
            {
                text = "test text no 1.",
                x = 150,
                y = 100,
                width = 130,
                height = 200,
                fontSize = 25,
                color = "#FF00008B",
                showborder = true,
                page = 1,
                type = PDFField.PDFFieldType.TextField
            });
            list.Add(new PdfTextField()
            {
                text = "test text no 2.",
                x = 210,
                y = 150,
                width = 130,
                height = 200,
                fontSize = 15,
                color = "#FF00008B",
                showborder = true,
                page = 1,
                type = PDFField.PDFFieldType.TextField
            });
            list.Add(new PdfSignatureField()
            {
                type = PDFField.PDFFieldType.SignatureField,
                name = "signature1",
                x = 100,
                y = 100,
                width = 100,
                height = 100,
                reason = "test signature 1",
                location = "locationJa",
                page = 1,
                thumbprint = "‎76 61 4a 24 85 76 46 4a 5b 13 75 3a e4 f3 31 4b 7b aa 79 62".Replace("‎", "").Replace(" ", "").ToUpper()
            });
            list.Add(new PdfSignatureField()
            {
                type = PDFField.PDFFieldType.SignatureField,
                name = "signature2",
                x = 200,
                y = 200,
                width = 100,
                height = 100,
                reason = "test signature 2",
                location = "locationJa2",
                page = 1,
                thumbprint = "‎bc 97 b6 69 77 48 9c fb ca a0 78 58 38 19 c5 d6 1f 65 0c b8".Replace("‎", "").Replace(" ", "").ToUpper()
            });
            return list;
        }
        [TestMethod()]
        public void AddTemplateFieldsTest()
        {
            var controller = new PdfFieldController();
            var request = new AddFieldsRequest()
            {
                template_id = "79d53f5e-11cb-4431-82b9-d92fa06fa322",
                path_in = @"D:\temp\testSignatureService\BG.pdf",
                path_out = @"D:\temp\testSignatureService\BG_from_template.pdf"
            };
            controller.AddFields(request);
        }
    }
}