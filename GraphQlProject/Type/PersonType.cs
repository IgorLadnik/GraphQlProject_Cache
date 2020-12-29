using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.Generic;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;

namespace GraphQlProject.Type
{
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType(GraphQLDbContext dbContext)
        {       
            Field(p => p.Id);
            Field(p => p.StrId);
            Field(p => p.GivenName);
            Field(p => p.Surname);
            Field(p => p.Born);
            Field(p => p.Phone);
            Field(p => p.Email);
            Field(p => p.Address);

            Field<ListGraphType<AffiliationType>>("affiliations", resolve: context =>
            {
                IList<Affiliation> affiliations;
                if (context.GetCache("affiliations") == null)
                {
                    var personIds = (IList<int>)context.GetCache("persons").Payload;
                    affiliations = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList();
                    context.SetCache("affiliations", new Cache { Payload = affiliations });
                }

                affiliations = (IList<Affiliation>)context.GetCache("affiliations").Payload;
                return affiliations.Where(a => a.PersonId == context.Source.Id);
            });

            Field<ListGraphType<RelationType>>("relations", resolve: context =>
            {
                IList<Relation> relations;
                if (context.GetCache("relations") == null)
                {
                    var personIds = (IList<int>)context.GetCache("persons").Payload;
                    relations = dbContext.Relations.Where(r => personIds.Contains(r.P1Id)).ToList();
                    context.SetCache("relations", new Cache { Payload = relations });
                }

                relations = (IList<Relation>)context.GetCache("relations").Payload;
                return relations.Where(r => r.P1Id == context.Source.Id);
            });
        }
    }
}
