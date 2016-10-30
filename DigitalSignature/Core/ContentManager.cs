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
        //public string CreateTemplate(string name, string description, List<PDFField> pdfFields)
        //{
        //    var template = new DigitalSignatureTemplate() { name = name, description = description, pdfFields = pdfFields };
        //    SaveTemplate(template);
        //    return template.id;
        //}
        //public void UpdateTemplate(string id, string name, string description, List<PDFField> pdfFields)
        //{
        //    var template = new DigitalSignatureTemplate() { id = id, name = name, description = description, pdfFields = pdfFields };
        //    SaveTemplate(template);
        //}
        public abstract void DeleteTemplate(string id);
        public abstract void SaveTemplate(DigitalSignatureTemplate template);
        public abstract DigitalSignatureTemplate GetTemplate(string id);
        public abstract IEnumerable<DigitalSignatureTemplate> GetTemplates();
        public abstract IEnumerable<string> GetTemplateIds();
    }
}