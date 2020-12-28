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
                var cache = context.GetCache("personsInAffiliations");
                if (cache.IsFirstCall)
                {
                    cache.IsFirstCall = false;

                    // For the 1st time in this level
                    var personIds = (IList<int>)cache.Payload;

                    // Cache all data for the entire level
                    context.SetCache("affiliations",
                        // Fetch data from database for the entire level
                        new Cache { Payload = dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId))?.ToList() });
                }

                // Get entities from cache
                var affiliationsFromCache = (IList<Affiliation>)context.GetCache("affiliations").Payload;

                return affiliationsFromCache.Where(a => a.PersonId == context.Source.Id);
            });

            Field<ListGraphType<RelationType>>("relations", resolve: context =>
            {
                var cache = context.GetCache("personsInRelations");
                if (cache.IsFirstCall)
                {
                    cache.IsFirstCall = false;

                    var personIds = (IList<int>)cache.Payload;
                    context.SetCache("relations",
                        new Cache { Payload = dbContext.Relations.Where(r => personIds.Contains(r.P2Id))?.ToList() });
                }

                var relationsFromCache = (IList<Relation>)context.GetCache("relations").Payload;

                return relationsFromCache.Where(r => r.P1Id == context.Source.Id);
            });
        }
    }
}
