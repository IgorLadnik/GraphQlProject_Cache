using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using GraphQlHelperLib;
using GraphQL.Types;
using GraphQL;
using AuthRolesLib;

namespace GraphQlService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : GqlControllerBase
    {
        private GraphqlProcessor _gql;

        public PersonController(ISchema schema, IDocumentExecuter documentExecuter, IConfiguration configuration)
            : base(schema, documentExecuter, configuration)
        {
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

            return await ProcessQuery(query, UserAuthRole.SuperUser, UserAuthRole.Admin);
        }
    }
}
