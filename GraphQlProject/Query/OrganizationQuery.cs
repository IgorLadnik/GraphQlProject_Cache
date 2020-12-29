using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using GraphQlProject.Models;
using System.Collections.Generic;

namespace GraphQlProject.Query
{
    public class OrganizationQuery : ObjectGraphType
    {
        public OrganizationQuery(DbContextFactory dbContextFactory)
        {
            Field<ListGraphType<OrganizationType>>("organizations", resolve: context =>
            {
                IList<Organization> organizations;
                using (var dbContext = dbContextFactory.Create())
                    organizations = dbContext.Organizations.ToList();

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