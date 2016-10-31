using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestDigitalSignatureServiceApi
{
    public class Program
    {
        static void Main()
        {
            Task t = new Task(TestCreateTemplate);
            t.Start();
            Console.WriteLine("Starting ...");
            Console.ReadLine();
        }
        public static async void TestGetTemplates()
        {
            var address = @"http://localhost:7077/api/";
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
            var address = @"http://localhost:50740/api/";
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
            var address = @"http://172.20.36.201:7077/api/";
            var request = new
            {
                name = "TestNewTemplateJaa",
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
                thumbprint = "‎76 61 4a 24 85 76 46 4a 5b 13 75 3a e4 f3 31 4b 7b aa 79 62".Replace("‎", "").Replace(" ", "").ToUpper()
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
                thumbprint = "‎bc 97 b6 69 77 48 9c fb ca a0 78 58 38 19 c5 d6 1f 65 0c b8".Replace("‎", "").Replace(" ", "").ToUpper()
            });
            return list;
        }
    }
}
