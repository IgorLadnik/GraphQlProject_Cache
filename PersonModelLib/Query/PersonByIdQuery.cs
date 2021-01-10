using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Data;
using PersonModelLib.Type;

namespace PersonModelLib.Query
{
    [Authorize(Policy = "ApiUser")]
    public class PersonByIdQuery : ObjectGraphType
    {      
        public PersonByIdQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            FieldAsync<PersonType>("personById",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: async context =>
                    {
                        var user = context.GetUser();
                        var id = context.GetArgument<int>("id");
                        var person = await dbProvider.FetchAsync(dbContext => dbContext.Persons.Where(p => p.Id == id).FirstOrDefault());
                        if (person != null)
                            context.SetCache("personIds", new List<int> { person.Id });
                        return person;
                    });
        }
    }
}

/*
query PersonById {
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