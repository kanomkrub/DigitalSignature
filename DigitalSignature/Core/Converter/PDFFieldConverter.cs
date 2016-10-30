using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using DigitalSignatureService.Models;
using Newtonsoft.Json.Linq;
using static DigitalSignatureService.Models.PDFField;

namespace DigitalSignatureService.Core.Converter
{
    public class PDFFieldConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PDFField);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            if (jo["type"].Value<string>() == $"{PDFFieldType.TextField}")
                return jo.ToObject<PdfTextField>(serializer);

            if (jo["type"].Value<string>() == $"{PDFFieldType.SignatureField}")
                return jo.ToObject<PdfSignatureField>(serializer);
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // use default jsonconverter
        }
    }
}