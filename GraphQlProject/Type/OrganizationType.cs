using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;

namespace GraphQlProject.Type
{
    public class OrganizationType : ObjectGraphType<Organization>
    {
        public OrganizationType(GraphQLDbContext dbContext)
        {
            Field(o => o.Id);
            Field(o => o.StrId);
            Field(o => o.Name);
            Field(o => o.Address);

            Field<OrganizationType>("parent", resolve: context =>
            {
                var cache = context.GetCache("organizations");
                if (cache == null)
                    context.SetCache("organizations", new Cache { Payload = dbContext.Organizations.ToList() });

                var organizations = (IList<Organization>)context.GetCache("organizations").Payload;
                var thisOrganizationParentId = organizations.Where(o => o.Id == context.Source.Id).First().ParentId;
                return organizations.Where(o => o.Id == thisOrganizationParentId).FirstOrDefault();
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