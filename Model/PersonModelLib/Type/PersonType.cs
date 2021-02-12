using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Models;
using PersonModelLib.Data;
using RepoInterfaceLib;

namespace PersonModelLib.Type
{
    public class PersonType : ObjectGraphTypeCached<Person>
    {
        public PersonType(IRepo<GraphQLDbContext> repo, ILogger<ControllerBase> logger)
        {       
            Field(p => p.Id);
            Field(p => p.GivenName);
            Field(p => p.Surname);
            Field(p => p.Born);
            Field(p => p.Phone);
            Field(p => p.Email);
            Field(p => p.Address);

            FieldAsync<ListGraphType<AffiliationType>>("affiliations", resolve: async context =>
            {
                const string fieldName = "affiliations";
                var thisInstance = ++TraceHelper.instance;

                IList<Affiliation> affiliations;

                logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}before CacheDataFromRepo()");

                return await CacheDataFromRepo(
                    async () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), before check");

                        if (context.DoesCacheExist(fieldName))
                            return;

                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), the 1st, fetch");

                        var personIds = context.GetCache<IList<int>>("personIds");
                        affiliations = await repo.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                        context.SetCache<GqlCache>(fieldName, affiliations);
                    },
                    () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}after CacheDataFromRepo()");

                        affiliations = context.GetCache<IList<Affiliation>>(fieldName);
                        return affiliations.Where(a => a.PersonId == context.Source.Id);
                    },
                    logger, $"Type: PersonType, Field: {fieldName}");
            });

            FieldAsync<ListGraphType<RelationType>>("relations", resolve: async context =>
            {
                const string fieldName = "relations";
                var thisInstance = ++TraceHelper.instance;

                IList<Relation> relations;

                logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}before CacheDataFromRepo()");

                return await CacheDataFromRepo(
                    async () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), before check");

                        if (context.DoesCacheExist(fieldName))
                            return;

                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}inside CacheDataFromRepo(), the 1st, fetch");

                        var personIds = context.GetCache<IList<int>>("personIds");
                        relations = await repo.FetchAsync(dbContext => dbContext.Relations.Where(r => personIds.Contains(r.P1Id)).ToList());
                        context.SetCache<GqlCache>(fieldName, relations);
                    },
                    () =>
                    {
                        logger.LogTrace($"{TraceHelper.Out(fieldName, thisInstance)}after CacheDataFromRepo()");

                        relations = context.GetCache<IList<Relation>>(fieldName);
                        return relations?.Where(r => r.P1Id == context?.Source?.Id);
                    },
                    logger, $"Type: PersonType, Field: {fieldName}");
            });
        }
    }
}
