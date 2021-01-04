using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQlProject.Models;
using GraphQlProject.Data;
using GraphQlHelperLib;

namespace GraphQlProject.Type
{
    public class RelationType : ObjectGraphTypeCached<Relation>
    {
        public RelationType(DbProvider<GraphQLDbContext> dbProvider)
        {
            Field(r => r.Id);
            //Field(r => r.StrId);
            Field(r => r.Since);
            Field(r => r.Kind);
            Field(r => r.Notes);

            FieldAsync<PersonType>("p2", resolve: async context =>
                await Task.Run(async () =>
                {
                    var relations = context.GetCache<IList<Relation>>("relations");
                    IList<Person> persons;

                    await FirstCall(async () =>
                    {
                        var pIds = relations.Select(r => r.P2Id).ToList();
                        persons = await dbProvider.Fetch(dbContext => dbContext.Persons.Where(p => pIds.Contains(p.Id)).ToList());
                        context.SetCache("personsInRelations", persons);
                    });

                    persons = context.GetCache<IList<Person>>("personsInRelations");
                    var relation = relations.Where(r => r.Id == context.Source.Id).FirstOrDefault();
                    return persons.Where(p => p.Id == relation?.P2Id).FirstOrDefault();
                }));
        }
    }
}
