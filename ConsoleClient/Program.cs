using System;
using System.Threading.Tasks;
using HttpClientLib;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var id = -1;
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

            var loginUri = "https://localhost:5011/login";
            var gqlUri = "https://localhost:5001/gql";
            var userName = "Super";
            var password = "SuperPassword";

            HttpClientWrapper client = new();

            Task.Run(async () =>
            {
                string result = null;
                if (await client.LoginAsync(loginUri, userName, password) != null)
                    result = await client.PostAsync(gqlUri, query);
                Console.WriteLine(client.IsOK ? result : client.ErrorMessage);
            });

            Console.WriteLine("Press any key to quite...");
            Console.ReadKey();
        }
    }
}
