using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using GraphQlHelperLib;
using PersonModelLib.Models;
using PersonModelLib.Data;
using RepoInterfaceLib;

namespace PersonModelLib.Type
{
    public class OrganizationType : ObjectGraphTypeCached<Organization>
    {
        public OrganizationType(IRepo<GraphQLDbContext> repo, ILogger<ControllerBase> logger)
        {
            Field(o => o.Id);
            Field(o => o.Name);
            Field(o => o.Address);

            FieldAsync<OrganizationType>("parent", resolve: async context =>
            {
                const string cacheName = "parentOrganizations";
                IList<Organization> organizations;

                return await CacheDataFromRepo(
                    async () =>
                    {
                        if (context.DoesCacheExist(cacheName))
                            return;

                        organizations = await repo.FetchAsync(dbContext => dbContext.Organizations.ToList());
                        context.SetCache<GqlCache>(cacheName, organizations);
                    },
                    () =>
                    {
                        organizations = context.GetCache<IList<Organization>>(cacheName);
                        var thisOrganizationParentId = organizations.Where(o => o.Id == context.Source.Id).First().ParentId;
                        return organizations?.Where(o => o.Id == thisOrganizationParentId).FirstOrDefault();
                    },
                    logger, "Type: OrganizationType, Field: parent");
            });
        }
    }
}

