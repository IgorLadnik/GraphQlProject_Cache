using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Linq;
using System.Collections.Generic;

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
                IList<Affiliation> affiliations;
                if (context.GetCache("affiliations") == null)
                {
                    var personIds = (IList<int>)context.GetCache("personIds");
                    affiliations = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList();
                    context.SetCache("affiliations", affiliations);
                }

                affiliations = (IList<Affiliation>)context.GetCache("affiliations");
                if (context.GetCache("roles") == null)
                {
                    var organizationIds = affiliations.Select(a => a.OrganizationId).Distinct().ToList();
                    context.SetCache("organizations", dbContext.Organizations.Where(o => organizationIds.Contains(o.Id)).ToList());
                }

                var organizations = (IList<Organization>)context.GetCache("organizations");
                var organizationId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault().OrganizationId;
                return organizations.Where(o => o.Id == organizationId).FirstOrDefault();
            });

            Field<RoleType>("role", resolve: context =>
            {
                IList<Affiliation> affiliations;
                if (context.GetCache("affiliations") == null)
                {
                    var personIds = (IList<int>)context.GetCache("personIds");
                    affiliations = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList();
                    context.SetCache("affiliations", affiliations);
                }

                affiliations = (IList<Affiliation>)context.GetCache("affiliations");
                if (context.GetCache("roles") == null)
                {
                    var roleIds = affiliations.Select(a => a.RoleId).Distinct().ToList();
                    context.SetCache("roles", dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList());
                }

                var roles = (IList<Role>)context.GetCache("roles");
                var roleId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault().RoleId;
                return roles.Where(r => r.Id == roleId).FirstOrDefault();
            });
        }
    }
}