using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
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
            Field<ListGraphType<OrganizationType>>("organizations", resolve: context =>
            {
                var organizations = dbContext.Organizations;
                context.SetCache("organizations", new Cache { Payload = organizations.ToList() });
                return organizations;
            });
        }
    }
}

/*
query {
  organizationQuery {
    organizations {
      name
      parent {
        name
      }
    }
  }
}

*/