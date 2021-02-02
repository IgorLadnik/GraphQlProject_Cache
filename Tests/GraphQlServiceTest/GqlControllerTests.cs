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
    public class GqlControllerTests : IntegrationTest
    {
        const string query =
            @"query Traders {
              tradersQuery {
                traders(
                  isDeleted: false 
                    sortBy: '!Birthdate'
    	            pageSize: 2
    	            currentPage: 0) {
                    id
                    firstName
                    lastName
                    birthdate
                    email
                    password
                    isDeleted
                    cryptocurrencies
                        {
                            id
                            currency
                            symbol
                        }
                    }
                }
            }";

        const string mutation =
            @"mutation TradersMutation {
              tradersMutation {
                createTraders(
                  tradersInput: [
                  {
                    firstName: 'Lior'
                    lastName: 'Levy'
                    birthdate: '1950-01-01'
                    avatar: 'www.trader/member/images/llevy.png',
                    email: 'llevy@trader.com'
                    password: 'lll'
                    isDeleted: false
                    cryptocurrencies: [{ id: 1 }{ id: 2 }]
   	              }
                  {
                    firstName: 'Ann'
                    lastName: 'Linders'
                    birthdate: '1980-01-01'
                    email: 'annl@trader.com'
                    avatar: 'www.trader/member/images/annl.png',
                    password: 'lll'
                    isDeleted: false
                    cryptocurrencies:[{ id: 1 }
                { id: 1 }]
   	              }
                  ]
                ) {
                status
                message
                }
              }
            }";


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

        private HttpClientWrapper _client;

        public GqlControllerTests()
        {
            var testClient = new WebApplicationFactory<Startup>().CreateClient();
            _client = new(testClient);
        }

        [TestMethod]
        public async Task TestQuery()
        {
            var content = await Execute(query);

            var jObject = JObject.Parse(content);
            var t = jObject["tradersQuery"]["traders"][0];
            Assert.IsNotNull(t);
            Assert.IsTrue((int)t["id"] == -1);
            Assert.IsTrue((string)t["email"] == "mcohen@trader.com");
            t = t["cryptocurrencies"];
            Assert.IsTrue((int)t[2]["id"] == 3);

            t = jObject["tradersQuery"]["traders"][1];
            Assert.IsNotNull(t);
            Assert.IsTrue((int)t["id"] == -2);
            Assert.IsTrue((string)t["email"] == "vpupkin@trader.com");
            t = t["cryptocurrencies"];
            Assert.IsTrue((int)t[0]["id"] == 1);
        }

        [TestMethod]
        public async Task TestMutation()
        {
            var result = await Execute(mutation);
            var jObject = JObject.Parse(result);
            var jResult = jObject["tradersMutation"]["createTraders"];
            Assert.IsTrue((string)jResult["status"] == "Success");
            Assert.IsTrue((string)jResult["message"] == string.Empty);

            // In addition for complete test, newly inserted / updated data should by queried and checked here.
        }
    }
}
