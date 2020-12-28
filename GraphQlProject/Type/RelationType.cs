using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Linq;

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
                var relations = dbContext.Relations;
                var personId = relations.Where(r => r.Id == context.Source.Id).First().P2Id;
                return dbContext.Persons.Where(p => p.Id == personId).First();
            });
        }
    }
}
