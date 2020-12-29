using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQlProject.Type
{
    public class RelationType : ObjectGraphType<Relation>
    {
        private AutoResetEvent _ev;

        public RelationType(DbContextFactory dbContextFactory)
        {
            Field(r => r.Id);
            Field(r => r.StrId);
            Field(r => r.Since);
            Field(r => r.Kind);
            Field(r => r.Notes);

            _ev = new AutoResetEvent(true);

            FieldAsync<PersonType>("p2", resolve: async context =>
                await Task.Run(() =>
                {
                    var relations = (IList<Relation>)context.GetCache("relations");

                    _ev.WaitOne();
                    if (context.GetCache("personsInRelations") == null) 
                    {
                        var pIds = relations.Select(r => r.P2Id).ToList();
                        using (var dbContext = dbContextFactory.Create())
                            context.SetCache("personsInRelations", dbContext.Persons.Where(p => pIds.Contains(p.Id)).ToList());
                    }
                    _ev.Set();

                    var persons = (IList<Person>)context.GetCache("personsInRelations");
                    var relation = relations.Where(r => r.Id == context.Source.Id).FirstOrDefault();
                    return persons.Where(p => p.Id == relation?.P2Id).FirstOrDefault();
                }));
        }
    }
}
