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
                _contentStore = new TemplateFileStore(null);
            return _contentStore;
        }
        internal abstract string CreateTemplate(string name, string description, List<PDFField> pdfFields = null);
    }
}