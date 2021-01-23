using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var id = 2;
            var query = @"   
            {
                personByIdQuery {
                    personById(id: /*id*/) {
                    surname
                    relations {
                        p2 {
                        surname
                        }
                        kind
                    }
                    affiliations {
                        organization {
                        name
                        }
                        role {
                        name
                        }
                    }
                    }
                }
            }".Replace("/*id*/", $"{id}");

            Task.Run(async () => 
            {
                var loginUri = "https://localhost:5011/login/fromCode";
                var gqlUri = "https://localhost:5001/gql";
                var userName = "Super";
                var password = "SuperPassword";
                var token = await Login(loginUri, userName, password);
                var result = await QueryGraphQL(gqlUri, token, query);
                Console.WriteLine(result);
            });

            Console.WriteLine("Press any key to quite...");
            Console.ReadKey();
        }

        private static async Task<string> Login(string uri, string userName, string password)
        {
            HttpClient httpClient = new();
            StringContent stringContent = new(JsonConvert.SerializeObject($"?UserName={userName}&Password={password}"), Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(uri, stringContent);
            return res.IsSuccessStatusCode
                ? await res.Content.ReadAsStringAsync()
                : $"Error occurred... Status code:{res.StatusCode}";
        }

        private static async Task<string> QueryGraphQL(string uri, string token, string query)
        {
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var postData = new { Query = query };
            StringContent stringContent = new(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(uri, stringContent);
            return res.IsSuccessStatusCode
                ? await res.Content.ReadAsStringAsync()
                : $"Error occurred... Status code:{res.StatusCode}";
        }
    }
}
