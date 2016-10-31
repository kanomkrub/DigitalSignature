using DigitalSignatureService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalSignatureService.Core
{
    public abstract class ContentManager
    {
        private static ContentManager _contentStore;
        public static ContentManager GetInstance()
        {
            if (_contentStore == null)
                _contentStore = new TemplateFileStore();
            return _contentStore;
        }
        public string CreateTemplate(string name, string description, List<PDFField> pdfFields)
        {
            if (GetTemplates().Any(t => t.name == name)) throw new AccessViolationException($"duplicate name of '{name}'.");
            var template = new DigitalSignatureTemplate() { name = name, description = description, pdfFields = pdfFields };
            SaveTemplate(template);
            return template.id;
        }
        public string UpdateTemplate(string id, string name, string description, List<PDFField> pdfFields)
        {
            if (!GetTemplates().Any(t => t.id == id)) throw new AccessViolationException($"template id '{id}' not found.");
            var oldTemplate = GetTemplate(id);
            oldTemplate.name = name;
            oldTemplate.description = description;
            oldTemplate.last_modify_date = DateTime.Now;
            oldTemplate.pdfFields = pdfFields;
            SaveTemplate(oldTemplate);
            return id;
        }
        public abstract void DeleteTemplate(string id);
        public abstract void SaveTemplate(DigitalSignatureTemplate template);
        public abstract DigitalSignatureTemplate GetTemplate(string id);
        public abstract IEnumerable<DigitalSignatureTemplate> GetTemplates();
        public abstract IEnumerable<string> GetTemplateIds();
    }
}