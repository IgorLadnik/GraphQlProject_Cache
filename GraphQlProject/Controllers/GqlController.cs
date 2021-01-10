using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using JwtHelperLib;

namespace GraphQlProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GqlController : Controller
    {
        private GraphqlProcessor _gql;

        public GqlController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _gql = new(schema, documentExecuter);
        }

        [HttpPost]
        [Route("auth")]
        [AuthorizeRoles(UserAuthType.SuperUser)]
        public async Task<IActionResult> PostAsyncAuth([FromBody] GraphqlQuery query/*, 
                           [FromServices] IEnumerable<IValidationRule> validationRules*/) =>
            await ProcessQuery(query);

        [HttpPost]
        [Route("free")]
        public async Task<IActionResult> PostAsyncFree([FromBody] GraphqlQuery query) => await ProcessQuery(query);
        
        private async Task<IActionResult> ProcessQuery(GraphqlQuery query) 
        {
            var result = await _gql.Process(query, User);
            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
