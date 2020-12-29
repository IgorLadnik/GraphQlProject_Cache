using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GraphQlProject.Type
{ 
    public class AffiliationType : ObjectGraphType<Affiliation>
    {
        private AutoResetEvent _ev;

        
        public AffiliationType(DbContextFactory dbContextFactory)
        {
            Field(a => a.Id);
            Field(a => a.StrId);
            Field(a => a.Since);

            _ev = new AutoResetEvent(true);

            FieldAsync<OrganizationType>("organization", resolve: async context =>
                await Task.Run(() =>
                {
                    IList<Affiliation> affiliations;
                    IList<Organization> organizations;

                    _ev.WaitOne();
                    if (context.GetCache("affiliations") == null)
                    {
                        var personIds = (IList<int>)context.GetCache("personIds");
                        affiliations = dbContextFactory.FetchFromDb<IList<Affiliation>>(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                        context.SetCache("affiliations", affiliations);
                    }

                    affiliations = (IList<Affiliation>)context.GetCache("affiliations");
                    if (context.GetCache("roles") == null)
                    {
                        var organizationIds = affiliations.Select(a => a.OrganizationId).Distinct().ToList();
                        organizations = dbContextFactory.FetchFromDb<IList<Organization>>(dbContext => dbContext.Organizations.Where(o => organizationIds.Contains(o.Id)).ToList());
                        context.SetCache("organizations", organizations);
                    }
                    _ev.Set();

                    organizations = (IList<Organization>)context.GetCache("organizations");
                    var organizationId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault().OrganizationId;
                    return organizations.Where(o => o.Id == organizationId).FirstOrDefault();
                }));

            FieldAsync<RoleType>("role", resolve: async context =>
                await Task.Run(() =>
                {
                    IList<Affiliation> affiliations;

                    _ev.WaitOne();
                    if (context.GetCache("affiliations") == null)
                    {
                        var personIds = (IList<int>)context.GetCache("personIds");
                        using (var dbContext = dbContextFactory.Create())
                            affiliations = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList();
                        context.SetCache("affiliations", affiliations);
                    }

                    affiliations = (IList<Affiliation>)context.GetCache("affiliations");
                    if (context.GetCache("roles") == null)
                    {
                        var roleIds = affiliations.Select(a => a.RoleId).Distinct().ToList();
                        using (var dbContext = dbContextFactory.Create())
                            context.SetCache("roles", dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList());
                    }
                    _ev.Set();

                    var roles = (IList<Role>)context.GetCache("roles");
                    var roleId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault().RoleId;
                    return roles.Where(r => r.Id == roleId).FirstOrDefault();
                }));
        }
    }
}