using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LUISIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            string query = String.Empty;
            do
            {
                Console.WriteLine("Question to send to LUIS:");
                query = Console.ReadLine();

                if(query == String.Empty)
                {
                    Console.WriteLine("Question cannot be empty.");
                }

                var response = MakeRequest(query).Result;
                Console.WriteLine($"Top scoring intent - {response?.TopScoringIntent?.Intent}");
                Console.WriteLine($"Intent score - {response?.TopScoringIntent?.Score}");

                Console.WriteLine($"Entity count - {response?.Entities.Count}");

                if(response?.Entities.Count > 0)
                {
                    Console.WriteLine("Entities:");

                    foreach(var entity in response?.Entities)
                    {
                        Console.WriteLine(entity.EntityName);
                    }
                }

                Console.WriteLine(Environment.NewLine);
            }
            while (!"q".Equals(query, StringComparison.InvariantCultureIgnoreCase));
        }

        private async static Task<LuisResponse> MakeRequest(string query)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var keys = GetKeys();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keys.SubscriptionKey);

            queryString["q"] = query;

            var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + keys.LuisAppId + "?" + queryString;
            var response = await client.GetAsync(uri);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LuisResponse>(responseContent);
        }

        private static Keys GetKeys()
        {
            Keys keys;

            using (var reader = new StreamReader("App.json"))
            {
                var json = reader.ReadToEnd();

                keys = JsonConvert.DeserializeObject<Keys>(json);
            }

            return keys;
        }
    }
}
