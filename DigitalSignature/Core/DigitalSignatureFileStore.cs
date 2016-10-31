using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DigitalSignatureService.Models;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;

namespace DigitalSignatureService.Core
{
    public class TemplateFileStore : ContentManager
    {
        private string templateRepository;
        private string encryptionKey;
        public TemplateFileStore()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["templateStore"]))
                templateRepository = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TechSphere\DigitalSignature\Templates";
            else
                templateRepository = ConfigurationManager.AppSettings["templateStore"];
            if (!Directory.Exists(templateRepository)) Directory.CreateDirectory(templateRepository);
        }
        public override void SaveTemplate(DigitalSignatureTemplate template)
        {
            var path = $"{templateRepository}\\{template.id}.json";
            var json = JsonConvert.SerializeObject(template);
            var bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);
        }
        public override void DeleteTemplate(string id)
        {
            var path = $"{templateRepository}\\{id}.json";
            if (File.Exists(path))
                File.Delete(path);
        }

        public override DigitalSignatureTemplate GetTemplate(string id)
        {
            var path = $"{templateRepository}\\{id}.json";
            if (!File.Exists(path)) throw new KeyNotFoundException($"template {id} not found.");
            var content = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<DigitalSignatureTemplate>(content, new Converter.PDFFieldConverter());
        }

        public override IEnumerable<DigitalSignatureTemplate> GetTemplates()
        {
            foreach (var file in Directory.GetFiles(templateRepository))
            {
                var content = File.ReadAllText(file);
                yield return JsonConvert.DeserializeObject<DigitalSignatureTemplate>(content, new Converter.PDFFieldConverter());
            }
        }

        public override IEnumerable<string> GetTemplateIds()
        {
            foreach (var file in Directory.GetFiles(templateRepository))
            {
                yield return Path.GetFileNameWithoutExtension(file);
            }
        }
    }
}