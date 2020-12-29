using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using System.Collections.Generic;
using GraphQL;
using GraphQlProject.Models;

namespace GraphQlProject.Query
{
    public class PersonByIdQuery : ObjectGraphType
    {
        public PersonByIdQuery(DbContextFactory dbContextFactory)
        {
            Field<PersonType>("personById",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    Person person;
                    using (var dbContext = dbContextFactory.Create())
                        person = dbContext.Persons.Where(p => p.Id == id).FirstOrDefault();

                    if (person != null)
                        context.SetCache("personIds", new List<int> { person.Id });
                    return person;
                });
        }
    }
}

/*
query
{
    personByIdQuery {
      personById(id: 1) {
        surname
        relations {
          p2 {
            surname
          }
          kind
        }
        affiliations {
          organization {
            name
          }
          role {
            name
          }
        }
      }
    }
}
*/