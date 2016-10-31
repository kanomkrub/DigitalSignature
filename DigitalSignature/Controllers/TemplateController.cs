using DigitalSignatureService.Core;
using DigitalSignatureService.Models;
using System;
using System.Collections.Generic;
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
            var manager = ContentManager.GetInstance();
            manager.DeleteTemplate(id);
        }
        [HttpPost][HttpGet]
        [ActionName("get")]
        public IEnumerable<DigitalSignatureTemplate> GetTemplates([FromUri]string id = null)
        {
            var manager = ContentManager.GetInstance();
            var templates = manager.GetTemplates();
            if (!string.IsNullOrEmpty(id)) templates = templates.Where(t => t.id == id);
            return templates;
        }
    }
}
