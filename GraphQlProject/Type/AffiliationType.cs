using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQlProject.Models;
using GraphQlProject.Data;
using GraphQlHelperLib;

namespace GraphQlProject.Type
{ 
    public class AffiliationType : ObjectGraphTypeCached<Affiliation>
    {               
        public AffiliationType(DbProvider<GraphQLDbContext> dbProvider)
        {
            Field(a => a.Id);
            //Field(a => a.StrId);
            Field(a => a.Since);

            FieldAsync<OrganizationType>("organization", resolve: async context =>
                await Task.Run(async () =>
                {
                    IList<Affiliation> affiliations = null;
                    IList<Organization> organizations;

                    await FirstCall(async () =>
                    {
                        if (!context.DoesCacheExist("affiliations"))
                        {
                            var personIds = context.GetCache<IList<int>>("personIds");
                            affiliations = await dbProvider.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                            context.SetCache("affiliations", affiliations);
                        }

                        affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                        if (!context.DoesCacheExist("organizations"))
                        {
                            var organizationIds = affiliations.Select(a => a.OrganizationId).Distinct().ToList();
                            organizations = await dbProvider.FetchAsync(dbContext => dbContext.Organizations.Where(o => organizationIds.Contains(o.Id)).ToList());
                            context.SetCache("organizations", organizations);
                        }
                    });

                    affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                    organizations = context.GetCache<IList<Organization>>("organizations");
                    var organizationId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault().OrganizationId;
                    return organizations.Where(o => o.Id == organizationId).FirstOrDefault();
                }));

            FieldAsync<RoleType>("role", resolve: async context =>
                await Task.Run(async () =>
                {
                    IList<Affiliation> affiliations = null;
                    IList<Role> roles;

                    await FirstCall(async () =>
                    {
                        if (!context.DoesCacheExist("affiliations"))
                        {
                            var personIds = context.GetCache<IList<int>>("personIds");
                            affiliations = await dbProvider.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                            context.SetCache("affiliations", affiliations);
                        }

                        affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                        if (!context.DoesCacheExist("roles"))
                        {
                            var roleIds = affiliations.Select(a => a.RoleId).Distinct().ToList();
                            roles = await dbProvider.FetchAsync(dbContext => dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList());
                            context.SetCache("roles", roles);
                        }
                    });

                    affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                    roles = context.GetCache<IList<Role>>("roles");
                    var roleId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault().RoleId;
                    return roles.Where(r => r.Id == roleId).FirstOrDefault();
                }));
        }
    }
}