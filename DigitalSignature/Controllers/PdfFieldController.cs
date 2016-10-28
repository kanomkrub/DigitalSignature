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
            public string path_in { get; set; }
            public string path_out { get; set; }
            public byte[] bytes { get; set; }
            public List<PDFField> fields { get; set; }
        }
        [HttpGet]
        [ActionName("add")]
        public object AddFields([FromBody]AddFieldsRequest request)
        {
            if (request.bytes != null)
                return PDFUtility.AddPDFFields(request.bytes, request.fields);
            else
                return PDFUtility.AddPDFFields(request.path_in, request.path_out, request.fields);
        }
    }
}
