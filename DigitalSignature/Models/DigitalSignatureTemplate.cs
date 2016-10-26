using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalSignatureService.Models
{
    public class DigitalSignatureTemplate
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<PDFField> pdfFields { get; set; }
    }
    public abstract class PDFField
    {
        public string name { get; set; }
        public string description { get; set; }
        public float x { get; set; } = 100;
        public float y { get; set; } = 100;
        public float width { get; set; } = 150;
        public float height { get; set; } = 300;
        public PDFFieldType type = PDFFieldType.TextField;
        public PDFPage page = PDFPage.First;
        public enum PDFPage
        {
            All,First,Last,Even,Odd,SpecificPage
        }
        public enum PDFFieldType
        {
            TextField,ImageField,SignatureField
        }
    }
    public class PdfTextField : PDFField
    {
        public int fontSize { get; set; } = 14;
        public string fontName { get; set; } = "HELVETICA_BOLD";
        public int fontstyle { get; set; } = Font.NORMAL;
        public string fontEncoding { get; set; } = "CP1252";
        public bool fontEmbeded { get; set; } = true;
        /// <summary>
        /// argb format
        /// </summary>
        public string color { get; set; } = "#FF000000"; //black
        public int align { get; set; }
        public string text { get; set; }
        public bool showborder { get; set; }
    }
    public class PdfImageField : PDFField
    {

    }
    public class PdfSignatureField : PDFField
    {


    }
}