using System.Linq;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Data;
using PersonModelLib.Type;

namespace PersonModelLib.Query
{
    public class OrganizationQuery : ObjectGraphType
    {
        public OrganizationQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            FieldAsync<ListGraphType<OrganizationType>>("organizations", resolve: async context =>
                {
                    var organizations = await dbProvider.FetchAsync(dbContext => dbContext.Organizations.ToList());
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