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
        [ActionName("create")]
        public string Create([FromBody]DigitalSignatureTemplateRequest templateRequest)
        {
            var manager = ContentManager.GetInstance();
            if (manager.GetTemplates().Any(t => t.name == templateRequest.name)) throw new AccessViolationException($"duplicate name of '{template.name}'.");
            var template = new DigitalSignatureTemplate()
            {
                id = Guid.NewGuid().ToString(),
                name = templateRequest.name,
                description = templateRequest.description,
                pdfFields = templateRequest.fields
            };
            manager.SaveTemplate(template);
            return template.id;
        }
        [ActionName("update")]
        public string Update([FromBody]DigitalSignatureTemplateRequest templateRequest)
        {
            var manager = ContentManager.GetInstance();
            var oldTemplate = manager.GetTemplate(templateRequest.id);
            //oldTemplate.id = Guid.NewGuid().ToString();
            oldTemplate.name = templateRequest.name;
            oldTemplate.description = templateRequest.description;
            oldTemplate.last_modify_date = DateTime.Now;
            oldTemplate.pdfFields = templateRequest.fields;
            manager.SaveTemplate(oldTemplate);
            return oldTemplate.id;
        }
    }
}
