using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestDigitalSignatureServiceApi
{
    public class Program
    {
        static string address = @"http://localhost:50740/api/";
        //static string address = @"http://172.20.36.201:7077/api/";
        static void Main()
        {
            Task t = new Task(TestRenderPdf);
            t.Start();
            Console.WriteLine("Starting ...");
            Console.ReadLine();
        }
        public static async void TestGetTemplates()
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(address+"template/get"))
            using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                dynamic templates = JToken.Parse(result);
                foreach (dynamic template in templates)
                {
                    Console.Out.WriteLine($"{template.name}");
                    Console.Out.WriteLine($"{template}");
                    Console.Out.WriteLine();
                }
            }
        }

        public static async void TestStampPdf()
        {
            var request = new
            {
                template_id = "5968aa8b-98f5-4793-92b6-cbbfb1ca8b6b",
                path_in = @"D:\temp\testSignatureService\BG.pdf",
                path_out = @"D:\temp\testSignatureService\BG_Out_FROM_API.pdf"
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json"); 
                
            using (var client = new HttpClient())
            using (var response = await client.PostAsync(address + "pdffield/addfields", requestContent))
            using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                Console.Out.WriteLine(result);
            }
        }

        public static async void TestCreateTemplate()
        {
            var request = new
            {
                name = "TestNewTemplateJaa22",
                description = "descriptionJaa",
                fields = GetTestList()
            };
            var requestText = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(requestText, System.Text.Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(address + "template/create", requestContent))
            using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                Console.Out.WriteLine(result);
            }
        }
        public static async void TestSetExamplePdf()
        {
            var bytes = File.ReadAllBytes(@"D:\temp\testSignatureService\bg.pdf");
            var requestContent = new ByteArrayContent(bytes);
            using (var client = new HttpClient())
            using (var response = await client.PostAsync(address + "template/setexamplepdf?template_id=xxxxx", requestContent))
            using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                Console.Out.WriteLine(result);
            }
        }
        public static async void TestGetExamplePdf()
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(address + "template/getexamplepdf?template_id=xxxxx"))
            using (var content = response.Content)
            {
                var result = content.ReadAsStringAsync().Result.Trim('"');
                response.EnsureSuccessStatusCode();
                var bytes = Convert.FromBase64String(result);
                File.WriteAllBytes(@"D:\temp\testSignatureService\BG_from_api.pdf", bytes);
            }
        }
        public static async void TestRenderPdf()
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(address + "template/renderexamplepdf?template_id=xxxxx"))
            using (var content = response.Content)
            {
                var result = content.ReadAsStringAsync().Result.Trim('"');
                response.EnsureSuccessStatusCode();
                var bytes = Convert.FromBase64String(result);
                File.WriteAllBytes(@"D:\temp\testSignatureService\BG_from_api.jpg", bytes);
            }
        }
        public static List<object> GetTestList()
        {
            var list = new List<object>();
            list.Add(new 
            {
                text = "test text no 1.",
                x = 150,
                y = 100,
                width = 130,
                height = 200,
                fontSize = 25,
                color = "#FF00008B",
                showborder = true,
                page = 1,
                type = "TextField"
            });
            list.Add(new 
            {
                text = "test text no 2.",
                x = 210,
                y = 150,
                width = 130,
                height = 200,
                fontSize = 15,
                color = "#FF00008B",
                showborder = true,
                page = 1,
                type = "TextField"
            });
            list.Add(new 
            {
                type = "SignatureField",
                name = "signature1",
                x = 100,
                y = 100,
                width = 100,
                height = 100,
                reason = "test signature 1",
                location = "locationJa",
                page = 1,
                thumbprint = "‎9e cf 30 18 0c d5 86 8a 3a 32 fb 7c a3 cf a7 9d 1a bb 5d 5d".Replace("‎", "").Replace(" ", "").ToUpper()
            });
            list.Add(new 
            {
                type = "SignatureField",
                name = "signature2",
                x = 200,
                y = 200,
                width = 100,
                height = 100,
                reason = "test signature 2",
                location = "locationJa2",
                page = 1,
                thumbprint = "79 71 3c b3 43 b1 18 03 f1 41 8a 29 06 c2 f1 45 92 7e 8a d0".Replace("‎", "").Replace(" ", "").ToUpper()
            });
            return list;
        }
    }
}
