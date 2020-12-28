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
                var cachePersonIds = context.GetCache("personIdsInAffiliations");
                var cache = context.GetCache("organizations");
                if (cachePersonIds != null && cache == null)
                {
                    var personIds = (IList<int>)cachePersonIds.Payload;
                    var organizationIds = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).Select(a => a.OrganizationId).Distinct().ToList();
                    context.SetCache("organizations", new Cache 
                    { 
                        Payload = dbContext.Organizations.Where(o => organizationIds.Contains(o.Id)).ToList(), 
                        IsFirstCall = false 
                    });
                }

                var organizations = (IList<Organization>)context.GetCache("organizations").Payload;

                return organizations.Where(o => o.Id == context.Source.Id).FirstOrDefault();
            });

            Field<RoleType>("role", resolve: context =>
            {
                var cachePersonIds = context.GetCache("personIdsInAffiliations");
                var cache = context.GetCache("roles");
                if (cachePersonIds != null && cache == null)
                {
                    var personIds = (IList<int>)cachePersonIds.Payload;
                    var roleIds = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).Select(a => a.RoleId).Distinct().ToList();
                    context.SetCache("roles", new Cache
                    {
                        Payload = dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList(),
                        IsFirstCall = false
                    });
                }

                var roles = (IList<Role>)context.GetCache("roles").Payload;

                return roles.Where(r => r.Id == context.Source.Id).FirstOrDefault();
            });
        }
    }
}