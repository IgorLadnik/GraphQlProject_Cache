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
    public class RelationType : ObjectGraphTypeCached<Relation>
    {
        public RelationType(IRepo<GraphQLDbContext> repo, ILogger<ControllerBase> logger)
        {
            Field(r => r.Id);
            Field(r => r.Since);
            Field(r => r.Kind);
            Field(r => r.Notes);

            FieldAsync<PersonType>("p2", resolve: async context =>
            {
                var relations = context.GetCache<IList<Relation>>("relations");
                IList<Person> persons;

                return await CacheDataFromRepo(
                    async () =>
                    {
                        var pIds = relations.Select(r => r.P2Id).ToList();
                        persons = await repo.FetchAsync(dbContext => dbContext.Persons.Where(p => pIds.Contains(p.Id)).ToList());
                        context.SetCache<GqlCache>("personsInRelations", persons);
                    },
                    () =>
                    {
                        persons = context.GetCache<IList<Person>>("personsInRelations");
                        var relation = relations.Where(r => r.Id == context.Source.Id).FirstOrDefault();
                        return persons?.Where(p => p.Id == relation?.P2Id).FirstOrDefault();
                    },
                    logger, "Type: RelationType, Field: p2");
            });
        }
    }
}
