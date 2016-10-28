using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DigitalSignatureService.Models;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace DigitalSignatureService.Core
{
    public class TemplateFileStore : ContentManager
    {
        private string repository;
        private string encryptionKey;
        public TemplateFileStore(NameValueCollection parameter)
        {
            //repository = parameter["repository"];
            //encryptionKey = parameter["password"];
        }
        internal override string CreateTemplate(string name, string description, List<PDFField> pdfFields = null)
        {
            var path = "";
            var template = new DigitalSignatureTemplate() { name = name, description = description, pdfFields = pdfFields };

            var json = JsonConvert.SerializeObject(template);
            var bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);

            return template.id;
        }
    }
}