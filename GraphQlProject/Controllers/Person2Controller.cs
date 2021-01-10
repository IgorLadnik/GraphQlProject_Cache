using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json.Linq;
using GraphQlHelperLib;
using GraphQL.Types;
using GraphQL;

namespace GraphQlProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Person2Controller : ControllerBase
    {
        private GraphqlProcessor _gql;

        public Person2Controller(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _gql = new(schema, documentExecuter);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query =
                @"   
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

            var result = await _gql.Process(query, User);
            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
