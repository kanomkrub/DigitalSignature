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
    public class PdfFieldController : ApiController
    {
        public class AddFieldsRequest
        {
            public string template_id { get; set; }
            public string path_in { get; set; }
            public string path_out { get; set; }
            public byte[] bytes { get; set; }
            public List<PDFField> fields { get; set; }
        }
        [HttpPost][HttpGet]
        [ActionName("addfields")]
        public object AddFields([FromBody]AddFieldsRequest request)
        {
            var fields = request.fields;
            if (!string.IsNullOrEmpty(request.template_id))
                fields = ContentManager.GetInstance().GetTemplate(request.template_id).pdfFields;
            if (request.bytes != null)
                return PDFUtility.AddPDFFields(request.bytes, fields);
            else
                return PDFUtility.AddPDFFields(request.path_in, request.path_out, fields);
        }
    }
}
