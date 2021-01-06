using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using GraphQlProject.Models;
using System.Collections.Generic;
using GraphQlHelperLib;
using System.Threading.Tasks;

namespace GraphQlProject.Query
{
    public class OrganizationQuery : ObjectGraphType
    {
        public OrganizationQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            FieldAsync<ListGraphType<OrganizationType>>("organizations", resolve: async context =>
                await Task.Run(async () =>
                {
                    var organizations = await dbProvider.FetchAsync(dbContext => dbContext.Organizations.ToList());
                    context.SetCache("organizations", organizations.ToList());
                    return organizations;
                }));
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