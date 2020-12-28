using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
//using GraphQlProject.Interfaces;
using GraphQlProject.Type;
using GraphQL.Language.AST;
using GraphQlProject.Data;

namespace GraphQlProject.Query
{
    public class OrganizationQuery : ObjectGraphType
    {
        public OrganizationQuery(GraphQLDbContext dbContext)
        {
            const string name = "organizations";

            Field<ListGraphType<OrganizationType>>(name, resolve: context =>
            {
                var organizations = dbContext.Organizations;
                context.SetCache(name, new Cache { Payload = organizations.Select(o => o.Id).ToArray() });
                return organizations;
            });
        }
    }
}
