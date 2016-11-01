using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Ghostscript.NET.Rasterizer;

namespace DigitalSignatureService.Core
{
    /// <summary>
    /// render using ghostscript32 http://ghostscript.com/download/gsdnld.html
    /// </summary>
    public static class PdfRenderer
    {
        public static Image Render(byte[] pdfContent, int dpixy, int page = 1)
        {
            using(var stream = new MemoryStream(pdfContent))
            {
                return Render(stream, dpixy, page);
            }
        }
        public static Image Render(string pdfPath, int dpixy, int page = 1)
        {
            using (var stream = File.OpenRead(pdfPath))
            {
                return Render(stream, dpixy, page);
            }
        }
        public static Image Render(Stream pdfStream, int dpixy, int page =1)
        {
            var rastelizer = new GhostscriptRasterizer();
            rastelizer.Open(pdfStream);
            var result = rastelizer.GetPage(dpixy, dpixy, page);
            rastelizer.Close();
            return result;
        }
    }
}