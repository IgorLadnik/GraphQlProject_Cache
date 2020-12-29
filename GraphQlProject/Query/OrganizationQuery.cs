using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
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