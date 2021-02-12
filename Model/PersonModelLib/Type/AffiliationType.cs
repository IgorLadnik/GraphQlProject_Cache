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
    public class AffiliationType : ObjectGraphTypeCached<Affiliation>
    {               
        public AffiliationType(IRepo<GraphQLDbContext> repo, ILogger<ControllerBase> logger)
        {
            Field(a => a.Id);
            Field(a => a.Since);

            FieldAsync<OrganizationType>("organization", resolve: async context =>
            {
                const string fieldName = "organization";
                var thisInstance = ++TraceHelper.instance;

                IList<Affiliation> affiliations = null;
                IList<Organization> organizations;

                logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}before CacheDataFromRepo()");

                return await CacheDataFromRepo(
                    async () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), before check");

                        if (!context.DoesCacheExist("affiliations"))
                        {
                            logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), 'affiliations' the 1st, fetch");

                            var personIds = context.GetCache<IList<int>>("personIds");
                            affiliations = await repo.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                            context.SetCache<GqlCache>("affiliations", affiliations);
                        }

                        affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                        if (!context.DoesCacheExist("organizations"))
                        {
                            logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), 'organizations' the 1st, fetch");

                            var organizationIds = affiliations.Select(a => a.OrganizationId).Distinct().ToList();
                            organizations = await repo.FetchAsync(dbContext => dbContext.Organizations.Where(o => organizationIds.Contains(o.Id)).ToList());
                            context.SetCache<GqlCache>("organizations", organizations);
                        }
                    },
                    () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}after CacheDataFromRepo()");

                        affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                        organizations = context.GetCache<IList<Organization>>("organizations");
                        var organizationId = affiliations?.Where(a => a.Id == context.Source.Id).FirstOrDefault().OrganizationId;
                        return organizations?.Where(o => o.Id == organizationId).FirstOrDefault();
                    },
                    logger, $"Type: AffiliationType, Field: {fieldName}");
            });

            FieldAsync<RoleType>("role", resolve: async context =>
            {
                const string fieldName = "role";
                var thisInstance = ++TraceHelper.instance;

                IList<Affiliation> affiliations = null;
                IList<Role> roles;

                logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}before CacheDataFromRepo()");

                return await CacheDataFromRepo(
                    async () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), before check");

                        if (!context.DoesCacheExist("affiliations"))
                        {
                            logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), 'affiliations' the 1st, fetch");

                            var personIds = context.GetCache<IList<int>>("personIds");
                            affiliations = await repo.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                            context.SetCache<GqlCache>("affiliations", affiliations);
                        }

                        affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                        if (!context.DoesCacheExist("roles"))
                        {
                            logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), 'roles' the 1st, fetch");

                            var roleIds = affiliations.Select(a => a.RoleId).Distinct().ToList();
                            roles = await repo.FetchAsync(dbContext => dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList());
                            context.SetCache<GqlCache>("roles", roles);
                        }
                    },
                    () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}after CacheDataFromRepo()");

                        affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                        roles = context.GetCache<IList<Role>>("roles");
                        var roleId = affiliations.Where(a => a.Id == context.Source.Id).FirstOrDefault()?.RoleId;
                        return roles?.Where(r => r.Id == roleId).FirstOrDefault();
                    },
                    logger, "Type: AffiliationType, Field: role");
            });
        }
    }
}