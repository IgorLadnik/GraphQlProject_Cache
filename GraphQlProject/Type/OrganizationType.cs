using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using GraphQlProject.Models;
using GraphQlProject.Data;
using GraphQlHelperLib;

namespace GraphQlProject.Type
{
    public class OrganizationType : ObjectGraphTypeCached<Organization>
    {
        public OrganizationType(DbProvider<GraphQLDbContext> dbProvider)
        {
            Field(o => o.Id);
            //Field(o => o.StrId);
            Field(o => o.Name);
            Field(o => o.Address);

            FieldAsync<OrganizationType>("parent", resolve: async context =>
                await Task.Run(async () =>
                {
                    const string cacheName = "parentOrganizations";
                    IList<Organization> organizations;

                    await FirstCall(async () =>
                    {
                        if (!context.DoesCacheExist(cacheName))
                        {
                            organizations = await dbProvider.FetchAsync(dbContext => dbContext.Organizations.ToList());
                            context.SetCache(cacheName, organizations);
                        }
                    });

                    organizations = context.GetCache<IList<Organization>>(cacheName);
                    var thisOrganizationParentId = organizations.Where(o => o.Id == context.Source.Id).First().ParentId;
                    return organizations.Where(o => o.Id == thisOrganizationParentId).FirstOrDefault();
                }));
        }
    }
}

