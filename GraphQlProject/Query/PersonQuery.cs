using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using GraphQlProject.Models;
using System.Collections.Generic;

namespace GraphQlProject.Query
{
    public class PersonQuery : ObjectGraphType
    {
        public PersonQuery(DbContextFactory dbContextFactory)
        {
            Field<ListGraphType<PersonType>>("persons", resolve: context =>
            {
                IList<Person> persons;
                using (var dbContext = dbContextFactory.Create())
                    persons = dbContext.Persons.ToList();

                context.SetCache("personIds", persons.Select(p => p.Id).ToList());
                return persons;
            });
        }
    }
}

/*
query
{
    personQuery {
        persons {
            givenName
            surname
            affiliations {
                organization {
                    name
                    parent {
                        name
                    }
                }
                role {
                    name
                }
            }
            relations {
                p2 {
                    givenName
                    surname
                }
                kind
                notes
            }
        }
    }
}
*/