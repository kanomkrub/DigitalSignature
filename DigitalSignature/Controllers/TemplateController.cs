using DigitalSignatureService.Core;
using DigitalSignatureService.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DigitalSignatureService.Controllers
{
    public class TemplateController : ApiController
    {
        public class DigitalSignatureTemplateRequest
        {
            public string id;
            public string name;
            public string description;
            public List<PDFField> fields;
        }
        [HttpPost]
        [ActionName("create")]
        public string Create([FromBody]DigitalSignatureTemplateRequest templateRequest)
        {
            var manager = ContentManager.GetInstance();
            return manager.CreateTemplate(templateRequest.name, templateRequest.description, templateRequest.fields);
        }
        [HttpPost]
        [ActionName("update")]
        public string Update([FromBody]DigitalSignatureTemplateRequest templateRequest)
        {
            var manager = ContentManager.GetInstance();
            return manager.UpdateTemplate(templateRequest.id, templateRequest.name, templateRequest.description, templateRequest.fields);
        }
        [HttpPost][HttpDelete]
        [ActionName("delete")]
        public void Delete([FromUri]string id)
        {
            ContentManager.GetInstance().DeleteTemplate(id);
        }
        [HttpPost][HttpGet]
        [ActionName("get")]
        public IEnumerable<DigitalSignatureTemplate> GetTemplates([FromUri]string id = null, [FromUri]string name = null)
        {
            var templates = ContentManager.GetInstance().GetTemplates();
            if (!string.IsNullOrEmpty(id)) templates = templates.Where(t => t.id == id);
            if (!string.IsNullOrEmpty(name)) templates = templates.Where(t => t.name == name);
            return templates;
        }

        [HttpPost]
        [ActionName("setexamplepdf")]
        public object SetExamplePdf(HttpRequestMessage request)
        {
            var template_id = request.GetQueryNameValuePairs().Single(t => t.Key == "template_id").Value;
            var bytes = request.Content.ReadAsByteArrayAsync().Result;
            var pdfInfo = PDFUtility.GetPdfInfo(bytes);
            ContentManager.GetInstance().SetExamplePdf(template_id, bytes);
            return pdfInfo;
        }
        [HttpGet][HttpPost]
        [ActionName("getexamplepdf")]
        public byte[] GetExamplePdf([FromUri]string template_id)
        {
            return ContentManager.GetInstance().GetExamplePdf(template_id);
        }
        [HttpGet][HttpPost]
        [ActionName("getpdfinfo")]
        public object GetPdfInfo(HttpRequestMessage request)
        {
            var bytes = request.Content.ReadAsByteArrayAsync().Result;
            return PDFUtility.GetPdfInfo(bytes);
        }
        [HttpGet][HttpPost]
        [ActionName("renderexamplepdf")]
        public byte[] RenderPdf([FromUri]string template_id, [FromUri]int dpi = 360, [FromUri]int page = 1)
        {
            var pdfContent = ContentManager.GetInstance().GetExamplePdf(template_id);
            var image = PdfRenderer.Render(pdfContent, dpi, page);
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }
    }
}
