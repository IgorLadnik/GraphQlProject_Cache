using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;

namespace GraphQlProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Person1Controller : Controller
    {
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            using var graphQLClient = new GraphQLHttpClient("https://localhost:5001/graphql", new NewtonsoftJsonSerializer());
            GraphQL.GraphQLRequest query = new()
            {
                Query = @"   
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
                }".Replace("/*id*/", $"{id}")
            };

            return (await graphQLClient.SendQueryAsync<JObject>(query)).Data.ToString();
        }
    }
}
