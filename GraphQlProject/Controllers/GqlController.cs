using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GraphQL;
using Newtonsoft.Json.Linq;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using GraphQlHelperLib;
using System.Collections.Generic;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authorization;
using GraphQlProject.Models;

namespace GraphQlProject.Controllers
{
    [Route("gql")]
    public class GqlController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        public GqlController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        [HttpPost]
        [AuthorizeRoles(UserType.SuperUser)]
        public async Task<IActionResult> PostAsync([FromBody] GraphqlQuery query/*, 
                           [FromServices] IEnumerable<IValidationRule> validationRules*/)
        {
            if (query == null) 
                throw new ArgumentNullException(nameof(query));
           
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                //ValidationRules = validationRules
            };
            executionOptions.SetUser(User);

            var result = await _documentExecuter.ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }

    public class GraphqlQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
