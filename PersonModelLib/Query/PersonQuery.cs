using System.Linq;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Data;
using PersonModelLib.Type;

namespace PersonModelLib.Query
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