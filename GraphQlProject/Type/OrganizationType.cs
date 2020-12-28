using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Linq;

namespace GraphQlProject.Type
{
    public class OrganizationType : ObjectGraphType<Organization>
    {
        public OrganizationType(GraphQLDbContext dbContext/*IOrganization organizationService*/)
        {
            Field(o => o.Id);
            Field(o => o.StrId);
            Field(o => o.Name);
            Field(o => o.Address);
            Field<OrganizationType>("parent", resolve: context =>
                {
                    var organizations = dbContext.Organizations;
                    var thisOrganizationParentId = organizations.Where(o => o.Id == context.Source.Id).First().ParentId;
                    return organizations.Where(o => o.Id == thisOrganizationParentId).FirstOrDefault();
                });
        }
    }
}
