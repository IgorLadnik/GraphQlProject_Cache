using System.Net.Http;
using System.Threading.Tasks;
using GraphQlService;
using HttpClientLib;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace GraphQlServiceTest
{
    [TestClass]
    public class GqlControllerTests //: IntegrationTest
    {
        const string query =
            @"query PersonById {
                  personByIdQuery {
                    personById(id: -1) {
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
                }
            ";

        readonly string mutation =
            @"mutation PersonMutation {
                personMutation {
                createPersons(
                    personsInput: [{
                        givenName: 'Vasya'
                        surname: 'Pupkin'
                        born: 1990
                        phone: '111-222-333'
                        email: 'vpupkin@ua.com'
                        address: '21, Torn Street'

                        affiliations: [{
                        since: 2000
                        organizationId: -4
                        roleId: -1
                        }]
      		
                        relations: [{
                        since: 2017
                        kind: 'friend'
                        notes: '*!'
                        p2Id: -1
                        }]					   
  		            }
                    ]
                ) {
                status
                message
                }
                }
            }".Replace("'", "\"");

        //var loginUri = "https://localhost:5011/login/fromCode";
        //var gqlUri = "https://localhost:5001/gql";
        //var userName = "Super";
        //var password = "SuperPassword";

        //HttpClientWrapper client = new();

        //Task.Run(async () =>
        //    {
        //        string result = null;
        //        if (await client.Login(loginUri, userName, password) != null)
        //            result = await client.PostAsync(gqlUri, query);
        //Console.WriteLine(client.IsOK? result : client.ErrorMessage);
        //    });

        const string gqlUri = "https://localhost:7001/gql";

        const string token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IlN1cGVyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU3VwZXJVc2VyIiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxLyJ9.TfTE49H0xyA6ZAAVBsu3TwmCwbnOwuGzlF2k2B5yHTQ";

        private HttpClientWrapper _client;

        public GqlControllerTests()
        {
            _client = new(new WebApplicationFactory<Startup>().CreateClient(), token);
        }

        [TestMethod]
        public async Task TestQuery()
        {
            var content = await _client.PostAsync(gqlUri, query);

            //var jObject = JObject.Parse(content);
            //var t = jObject["tradersQuery"]["traders"][0];
            //Assert.IsNotNull(t);
            //Assert.IsTrue((int)t["id"] == -1);
            //Assert.IsTrue((string)t["email"] == "mcohen@trader.com");
            //t = t["cryptocurrencies"];
            //Assert.IsTrue((int)t[2]["id"] == 3);

            //t = jObject["tradersQuery"]["traders"][1];
            //Assert.IsNotNull(t);
            //Assert.IsTrue((int)t["id"] == -2);
            //Assert.IsTrue((string)t["email"] == "vpupkin@trader.com");
            //t = t["cryptocurrencies"];
            //Assert.IsTrue((int)t[0]["id"] == 1);
        }

        [TestMethod]
        public async Task TestMutation()
        {
            var result = await _client.PostAsync(gqlUri, mutation);

            //var jObject = JObject.Parse(result);
            //var jResult = jObject["tradersMutation"]["createTraders"];
            //Assert.IsTrue((string)jResult["status"] == "Success");
            //Assert.IsTrue((string)jResult["message"] == string.Empty);

            // In addition for complete test, newly inserted / updated data should by queried and checked here.
        }
    }
}
