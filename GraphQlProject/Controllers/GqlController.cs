using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using JwtHelperLib;

namespace GraphQlProject.Controllers
{
    [Route("gql")]
    [ApiController]
    public class GqlController : Controller
    {
        private GraphqlProcessor _gql;

        public GqlController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _gql = new(schema, documentExecuter);
        }

        [HttpPost]
        [AuthorizeRoles(UserAuthType.SuperUser)]
        public async Task<IActionResult> PostAsync([FromBody] GraphqlQuery query/*, 
                           [FromServices] IEnumerable<IValidationRule> validationRules*/)
        {
            var result = await _gql.Process(query, User);
            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}
