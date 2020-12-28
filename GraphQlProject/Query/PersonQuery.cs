using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
//using GraphQlProject.Interfaces;
using GraphQlProject.Type;
using GraphQL.Language.AST;
using GraphQlProject.Data;

namespace GraphQlProject.Query
{
    public class PersonQuery : ObjectGraphType
    {
        public PersonQuery(GraphQLDbContext dbContext)
        {
            const string name = "persons";

            Field<ListGraphType<PersonType>>(name, resolve: context =>
            {
                var persons = dbContext.Persons;
                context.SetCache(name, new Cache { Payload = persons.Select(p => p.Id).ToArray() });
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