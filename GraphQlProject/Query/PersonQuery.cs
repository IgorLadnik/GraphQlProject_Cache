using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;

namespace GraphQlProject.Query
{
    public class PersonQuery : ObjectGraphType
    {
        public PersonQuery(GraphQLDbContext dbContext)
        {
            Field<ListGraphType<PersonType>>("persons", resolve: context =>
            {
                var persons = dbContext.Persons;
                var payload = persons.Select(p => p.Id).ToList();
                context.SetCache("persons", new Cache { Payload = payload });
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