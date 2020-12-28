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
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType(GraphQLDbContext dbContext/*IPerson personService*/)
        {       
            Field(p => p.Id);
            Field(p => p.StrId);
            Field(p => p.GivenName);
            Field(p => p.Surname);
            Field(p => p.Born);
            Field(p => p.Phone);
            Field(p => p.Email);
            Field(p => p.Address);

            Field< ListGraphType<AffiliationType>>("affiliations", resolve: context =>
            {
                var affiliations = dbContext.Affiliations;
                return affiliations.Where(a => a.PersonId == context.Source.Id);
            });

            Field<ListGraphType<RelationType>>("relations", resolve: context =>
            {
                var relations = dbContext.Relations;
                return relations.Where(r => r.P2Id == context.Source.Id);
            });
        }
    }
}
