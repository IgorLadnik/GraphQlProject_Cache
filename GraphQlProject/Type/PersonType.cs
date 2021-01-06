using System;
using System.Linq;
using System.Collections.Generic;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Threading.Tasks;
using GraphQlHelperLib;

namespace GraphQlProject.Type
{
    public class PersonType : ObjectGraphTypeCached<Person>
    {
        public PersonType(DbProvider<GraphQLDbContext> dbProvider)
        {       
            Field(p => p.Id);
            //Field(p => p.StrId);
            Field(p => p.GivenName);
            Field(p => p.Surname);
            Field(p => p.Born);
            Field(p => p.Phone);
            Field(p => p.Email);
            Field(p => p.Address);

            FieldAsync<ListGraphType<AffiliationType>>("affiliations",  resolve: async context =>
                {
                    IList<Affiliation> affiliations;

                    Console.WriteLine("before 1");

                    await FirstCall(async () =>
                    {
                        if (!context.DoesCacheExist("affiliations"))
                        {
                            Console.WriteLine("** fetch 1");

                            var personIds = context.GetCache<IList<int>>("personIds");
                            affiliations = await dbProvider.FetchAsync(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                            context.SetCache("affiliations", affiliations);
                        }
                    });

                    Console.WriteLine("after 1");

                    affiliations = context.GetCache<IList<Affiliation>>("affiliations");
                    return affiliations.Where(a => a.PersonId == context.Source.Id);
                });

            FieldAsync<ListGraphType<RelationType>>("relations", resolve: async context =>
                {
                    IList<Relation> relations;

                    Console.WriteLine("before 2");

                    await FirstCall(async () =>
                    {
                        if (!context.DoesCacheExist("relations"))
                        {
                            Console.WriteLine("** fetch 2");

                            var personIds = context.GetCache<IList<int>>("personIds");
                            relations = await dbProvider.FetchAsync(dbContext => dbContext.Relations.Where(r => personIds.Contains(r.P1Id)).ToList());
                            context.SetCache("relations", relations);
                        }
                    });

                    Console.WriteLine("after 2");

                    relations = context.GetCache<IList<Relation>>("relations");
                    return relations.Where(r => r.P1Id == context.Source.Id);
                });
        }
    }
}
