using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using GraphQlHelperLib;
using PersonModelLib.Models;
using PersonModelLib.Data;
using RepoInterfaceLib;

namespace PersonModelLib.Type
{
    public class OrganizationType : ObjectGraphTypeCached<Organization>
    {
        public OrganizationType(IRepo<GraphQLDbContext> repo)
        {
            Field(o => o.Id);
            //Field(o => o.StrId);
            Field(o => o.Name);
            Field(o => o.Address);

            FieldAsync<OrganizationType>("parent", resolve: async context =>
                {
                    const string cacheName = "parentOrganizations";
                    IList<Organization> organizations;

                    await FirstCall(async () =>
                    {
                        if (!context.DoesCacheExist(cacheName))
                        {
                            organizations = await repo.FetchAsync(dbContext => dbContext.Organizations.ToList());
                            context.SetCache<GqlCache>(cacheName, organizations);
                        }
                    });

                    organizations = context.GetCache<IList<Organization>>(cacheName);
                    var thisOrganizationParentId = organizations.Where(o => o.Id == context.Source.Id).First().ParentId;
                    return organizations.Where(o => o.Id == thisOrganizationParentId).FirstOrDefault();
                });
        }
    }
}

