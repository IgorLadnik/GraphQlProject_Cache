using System.Linq;
using System.Collections.Generic;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQlProject.Type
{
    public class OrganizationType : ObjectGraphType<Organization>
    {
        private AutoResetEvent _ev;

        public OrganizationType(DbContextFactory dbContextFactory)
        {
            Field(o => o.Id);
            Field(o => o.StrId);
            Field(o => o.Name);
            Field(o => o.Address);

            _ev = new AutoResetEvent(true);

            FieldAsync<OrganizationType>("parent", resolve: async context =>
                await Task.Run(() =>
                {
                    const string cacheName = "parentOrganizations";

                    _ev.WaitOne();
                    if (context.GetCache(cacheName) == null)
                    {
                        using (var dbContext = dbContextFactory.Create())
                            context.SetCache(cacheName, dbContext.Organizations.ToList());
                    }
                    _ev.Set();

                    var organizations = (IList<Organization>)context.GetCache(cacheName);
                    var thisOrganizationParentId = organizations.Where(o => o.Id == context.Source.Id).First().ParentId;
                    return organizations.Where(o => o.Id == thisOrganizationParentId).FirstOrDefault();
                }));
        }
    }
}

