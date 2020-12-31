using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using GraphQlProject.Models;
using System.Collections.Generic;
using GraphQlHelperLib;

namespace GraphQlProject.Query
{
    public class PersonQuery : ObjectGraphType
    {
        public PersonQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            Field<ListGraphType<PersonType>>("persons", resolve: context =>
            {
                var persons = dbProvider.Fetch<IList<Person>>(dbContext => dbContext.Persons.ToList());
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