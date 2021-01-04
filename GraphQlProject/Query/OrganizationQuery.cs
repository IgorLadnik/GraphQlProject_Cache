using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using GraphQlProject.Models;
using System.Collections.Generic;
using GraphQlHelperLib;

namespace GraphQlProject.Query
{
    public class OrganizationQuery : ObjectGraphType
    {
        public OrganizationQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            Field<ListGraphType<OrganizationType>>("organizations", resolve: context =>
            {
                var organizations = dbProvider.Fetch(dbContext => dbContext.Organizations.ToList());
                context.SetCache("organizations", organizations.ToList());
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