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
    public class AffiliationType : ObjectGraphType<Affiliation>
    {
        public AffiliationType(GraphQLDbContext dbContext)
        {
            Field(a => a.Id);
            Field(a => a.StrId);
            Field(a => a.Since);
            Field<OrganizationType>("organization", resolve: context =>
            {
                var affiliations = dbContext.Affiliations;
                var organizationId = affiliations.Where(a => a.Id == context.Source.Id).First().OrganizationId;
                return dbContext.Organizations.Where(o => o.Id == organizationId).First();
            });
            Field<RoleType>("role", resolve: context =>
            {
                var affiliations = dbContext.Affiliations;
                var roleId = affiliations.Where(a => a.Id == context.Source.Id).First().RoleId;
                return dbContext.Roles.Where(r => r.Id == roleId).First();
            });
        }
    }
}