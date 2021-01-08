using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using GraphQlProject.Models;
using System.Collections.Generic;
using GraphQlHelperLib;
using System.Threading.Tasks;

namespace GraphQlProject.Query
{
    public class PersonQuery : ObjectGraphType
    {
        public PersonQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            FieldAsync<ListGraphType<PersonType>>("persons", resolve: async context =>
                {
                    var persons = await dbProvider.FetchAsync(dbContext => dbContext.Persons.ToList());
                    context.SetCache("personIds", persons.Select(p => p.Id).ToList());
                    return persons;
                });
        }
    }
}

/*
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