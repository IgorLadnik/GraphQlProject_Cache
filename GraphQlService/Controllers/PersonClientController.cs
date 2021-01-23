//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json.Linq;
//using GraphQL.Client.Http;
//using GraphQL.Client.Serializer.Newtonsoft;

//namespace GraphQlService.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class PersonClientController : ControllerBase
//    {
//        [HttpGet("{id}")]
//        public async Task<string> Get(int id)
//        {
//            GraphQL.GraphQLRequest query = new()
//            {
//                Query = @"   
//                {
//                    personByIdQuery {
//                      personById(id: /*id*/) {
//                        surname
//                        relations {
//                          p2 {
//                            surname
//                          }
//                          kind
//                        }
//                        affiliations {
//                          organization {
//                            name
//                          }
//                          role {
//                            name
//                          }
//                        }
//                      }
//                    }
//                }".Replace("/*id*/", $"{id}")
//            };

//            using var graphQLClient = new GraphQLHttpClient("https://localhost:5001/graphql", new NewtonsoftJsonSerializer());
//            return (await graphQLClient.SendQueryAsync<JObject>(query)).Data.ToString();
//        }
//    }
//}
