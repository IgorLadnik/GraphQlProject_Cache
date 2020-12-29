using System;
using System.Linq;
using System.Collections.Generic;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Data;
using System.Threading.Tasks;
using System.Threading;

namespace GraphQlProject.Type
{
    public class PersonType : ObjectGraphType<Person>
    {
        private AutoResetEvent _ev;

        public PersonType(DbContextFactory dbContextFactory)
        {       
            Field(p => p.Id);
            Field(p => p.StrId);
            Field(p => p.GivenName);
            Field(p => p.Surname);
            Field(p => p.Born);
            Field(p => p.Phone);
            Field(p => p.Email);
            Field(p => p.Address);

            _ev = new AutoResetEvent(true);

            FieldAsync<ListGraphType<AffiliationType>>("affiliations",  resolve: async context =>
                await Task.Run(() =>
                {
                    IList<Affiliation> affiliations;

                    Console.WriteLine("before 1");

                    _ev.WaitOne();
                    if (context.GetCache("affiliations") == null)
                    {
                        Console.WriteLine("** fetch 1");

                        var personIds = (IList<int>)context.GetCache("personIds");
                        affiliations = dbContextFactory.FetchFromDb<IList<Affiliation>>(dbContext => dbContext.Affiliations.Where(a => personIds.Contains(a.PersonId)).ToList());
                        context.SetCache("affiliations", affiliations);
                    }
                    _ev.Set();

                    Console.WriteLine("after 1");

                    affiliations = (IList<Affiliation>)context.GetCache("affiliations");
                    return affiliations.Where(a => a.PersonId == context.Source.Id);
                }));

            FieldAsync<ListGraphType<RelationType>>("relations", resolve: async context =>
                await Task.Run(() =>
                {
                    IList<Relation> relations;

                    Console.WriteLine("before 2");

                    _ev.WaitOne();
                    if (context.GetCache("relations") == null)
                    {
                        Console.WriteLine("** fetch 2");

                        var personIds = (IList<int>)context.GetCache("personIds");
                        relations = dbContextFactory.FetchFromDb<IList<Relation>>(dbContext => dbContext.Relations.Where(r => personIds.Contains(r.P1Id)).ToList());
                        context.SetCache("relations", relations);
                    }
                    _ev.Set();

                    Console.WriteLine("after 2");

                    relations = (IList<Relation>)context.GetCache("relations");
                    return relations.Where(r => r.P1Id == context.Source.Id);
                }));
        }
    }
}
