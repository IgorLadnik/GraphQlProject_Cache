using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using HttpClientLib;

namespace GraphQlServiceTest
{
    [TestClass]
    public class Tests
    {
        const string query =
            @"query Persons {
              personQuery {
                persons {
                  id
                  givenName
                  surname
                  email
                  affiliations {
                    organization {
                      name
                      parent {
                        name
                      }
                    }
                    role {
                      name
                    }
                  }
                  relations {
                    p2 {
                      givenName
                      surname
                    }
                    kind
                    notes
                  }
                }
              }
            }";

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
                ) 
                {
                    status
                    message
                }
              }
            }".Replace("'", "\"");

        const string loginUri = "https://localhost:7000/login";
        const string gqlUri = "https://localhost:7000/gql";

        const string userName = "Super";
        const string password = "SuperPassword";

        const string token =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IlN1cGVyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU3VwZXJVc2VyIiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8iLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxLyJ9.TfTE49H0xyA6ZAAVBsu3TwmCwbnOwuGzlF2k2B5yHTQ";

        private HttpClientWrapper _clientLogin;
        private HttpClientWrapper _clientGql;

        public Tests()
        {
            _clientLogin = new(new WebApplicationFactory<LoginService.Startup>().CreateClient());
            _clientGql = new(new WebApplicationFactory<GraphQlService.Startup>().CreateClient(), token);
        }

        [TestMethod]
        public async Task TestLogin()
        {
            var strToken = await _clientLogin.LoginAsync(loginUri, userName, password);
            Assert.IsTrue(strToken == token);
        }

        [TestMethod]
        public async Task TestQuery()
        {
            var content = await _clientGql.PostAsync(gqlUri, query);

            var jObject = JObject.Parse(content);
            var t = jObject["personQuery"]["persons"][0];
            Assert.IsTrue((string)t["email"] == "feretti@ub.ac.zz");
        }

        [TestMethod]
        public async Task TestMutation()
        {
            var result = await _clientGql.PostAsync(gqlUri, mutation);

            var jObject = JObject.Parse(result);
            Assert.IsTrue((string)jObject["personMutation"]["createPersons"]["status"] == "Success");
        }
    }
}
