using System.Linq;
using GraphQL.Types;
using GraphQlProject.Type;
using GraphQlProject.Data;
using System.Collections.Generic;
using GraphQL;
using GraphQlProject.Models;
using GraphQlHelperLib;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace GraphQlProject.Query
{
    [Authorize(Policy = "ApiUser")]
    public class PersonByIdQuery : ObjectGraphType
    {      
        public PersonByIdQuery(DbProvider<GraphQLDbContext> dbProvider)
        {
            FieldAsync<PersonType>("personById",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: async context =>
                    await Task.Run(async () =>
                    {
                        var user = context.GetUser();
                        var id = context.GetArgument<int>("id");
                        var person = await dbProvider.FetchAsync(dbContext => dbContext.Persons.Where(p => p.Id == id).FirstOrDefault());
                        if (person != null)
                            context.SetCache("personIds", new List<int> { person.Id });
                        return person;
                    }));
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