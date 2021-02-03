using System.Linq;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Data;
using PersonModelLib.Type;
using RepoInterfaceLib;
using AuthRolesLib;

namespace PersonModelLib.Query
{
    public class PersonByIdQuery : ObjectGraphType
    {
        public PersonByIdQuery(IRepo<GraphQLDbContext> repo)
        {
            FieldAsync<PersonType>("personById",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: async context =>
                    {
                        context.ValidateRole(UserAuthRole.SuperUser, UserAuthRole.Admin); //TEST

                        var id = context.GetArgument<int>("id");
                        var person = await repo.FetchAsync(dbContext => dbContext.Persons.Where(p => p.Id == id).FirstOrDefault());
                        if (person != null)
                            context.SetCache<GqlCache>("personIds", new List<int> { person.Id });
                        return person;
                    });
        }
    }
}
