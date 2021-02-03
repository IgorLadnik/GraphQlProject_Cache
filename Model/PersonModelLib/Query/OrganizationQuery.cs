using System.Linq;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Data;
using PersonModelLib.Type;
using RepoInterfaceLib;

namespace PersonModelLib.Query
{
    public class OrganizationQuery : ObjectGraphType
    {
        public OrganizationQuery(IRepo<GraphQLDbContext> repo)
        {
            FieldAsync<ListGraphType<OrganizationType>>("organizations", resolve: async context =>
                {
                    var organizations = await repo.FetchAsync(dbContext => dbContext.Organizations.ToList());
                    context.SetCache<GqlCache>("organizations", organizations.ToList());
                    return organizations;
                });
        }
    }
}
