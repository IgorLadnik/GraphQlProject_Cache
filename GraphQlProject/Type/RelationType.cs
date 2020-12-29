using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Linq;
using System.Collections.Generic;

namespace GraphQlProject.Type
{
    public class RelationType : ObjectGraphType<Relation>
    {
        public RelationType(GraphQLDbContext dbContext)
        {
            Field(r => r.Id);
            Field(r => r.StrId);
            Field(r => r.Since);
            Field(r => r.Kind);
            Field(r => r.Notes);

            Field<PersonType>("p2", resolve: context =>
            {
                var relations = (IList<Relation>)context.GetCache("relations").Payload;
                if (context.GetCache("personsInRelations") == null) 
                {
                    var pIds = relations.Select(r => r.P2Id).ToList();
                    context.SetCache("personsInRelations", new Cache { Payload = dbContext.Persons.Where(p => pIds.Contains(p.Id)).ToList() });
                }

                var persons = (IList<Person>)context.GetCache("personsInRelations").Payload;
                var relation = relations.Where(r => r.Id == context.Source.Id).FirstOrDefault();
                return persons.Where(p => p.Id == relation?.P2Id).FirstOrDefault();
            });
        }
    }
}
