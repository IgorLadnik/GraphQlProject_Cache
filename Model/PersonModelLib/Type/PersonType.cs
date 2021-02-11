using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Models;
using PersonModelLib.Data;
using RepoInterfaceLib;

namespace PersonModelLib.Type
{
    public class PersonType : ObjectGraphTypeCached<Person>
    {
        static int instance = 0;

        public PersonType(IRepo<GraphQLDbContext> repo)
        {       
            Field(p => p.Id);
            Field(p => p.GivenName);
            Field(p => p.Surname);
            Field(p => p.Born);
            Field(p => p.Phone);
            Field(p => p.Email);
            Field(p => p.Address);

            FieldAsync<ListGraphType<AffiliationType>>("affiliations",  resolve: async context =>
                {
                    const string fieldName = "affiliations";
                    var thisInctance = ++instance;

                    IList<Affiliation> affiliations;

                    Console.WriteLine($"{TraceHelper.Out(fieldName, thisInctance)}before CacheDataFromRepo()");

                    await CacheDataFromRepo(async () =>
                    {
                        Console.WriteLine($"{TraceHelper.Out(fieldName, thisInctance)}inside CacheDataFromRepo(), before check");

                        if (context.DoesCacheExist(fieldName))
                            return;

                        Console.WriteLine($"{TraceHelper.Out(fieldName, thisInctance)}inside CacheDataFromRepo(), the 1st, fetch");

                        var personIds = context.GetCache<IList<int>>("personIds");
                        affiliations = await repo.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                        context.SetCache<GqlCache>(fieldName, affiliations);                       
                    });

                    Console.WriteLine($"{TraceHelper.Out(fieldName, thisInctance)}after CacheDataFromRepo()");

                    affiliations = context.GetCache<IList<Affiliation>>(fieldName);
                    return affiliations.Where(a => a.PersonId == context.Source.Id);
                });

            FieldAsync<ListGraphType<RelationType>>("relations", resolve: async context =>
                {
                    IList<Relation> relations;

                    await CacheDataFromRepo(async () =>
                    {
                        if (!context.DoesCacheExist("relations"))
                        {
                            var personIds = context.GetCache<IList<int>>("personIds");
                            relations = await repo.FetchAsync(dbContext => dbContext.Relations.Where(r => personIds.Contains(r.P1Id)).ToList());
                            context.SetCache<GqlCache>("relations", relations);
                        }
                    });

                    relations = context.GetCache<IList<Relation>>("relations");
                    return relations.Where(r => r.P1Id == context.Source.Id);
                });
        }
    }
}
