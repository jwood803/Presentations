using Newtonsoft.Json;
using System;
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

                }

                Console.WriteLine(Environment.NewLine);
            }
            while (!"q".Equals(query, StringComparison.InvariantCultureIgnoreCase));
        }

        private async static Task<LuisResponse> MakeRequest(string query)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var luisAppId = "1054ba52-b5c8-47ca-9df4-d658b0156bf6";
            var subscriptionKey = "99a453bd43d24bdf8e4cb12176a94301";

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            queryString["q"] = query;

            var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
            var response = await client.GetAsync(uri);

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LuisResponse>(responseContent);
        }
    }
}
