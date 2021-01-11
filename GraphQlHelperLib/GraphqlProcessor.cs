using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GraphQlHelperLib
{
    public class GraphqlProcessor
    {
        protected readonly IDocumentExecuter _documentExecuter;
        protected readonly ISchema _schema;

        public GraphqlProcessor(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        public async Task<ExecutionResult> Process(GraphqlQuery query, ClaimsPrincipal user/*, 
                           [FromServices] IEnumerable<IValidationRule> validationRules*/)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            //TEST
            if (!query.IsIntrospection)
            {
            }

            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs,
                //ValidationRules = validationRules
            };

            executionOptions.SetUser(user);

            return await _documentExecuter.ExecuteAsync(executionOptions);
        }

        public async Task<ExecutionResult> Process(string query, ClaimsPrincipal user)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query,
            };

            executionOptions.SetUser(user);

            return await _documentExecuter.ExecuteAsync(executionOptions);
        }
    }

    public class GraphqlQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }

        public bool IsIntrospection 
        {
            get => this.OperationName == "IntrospectionQuery";
        }
    }
}
