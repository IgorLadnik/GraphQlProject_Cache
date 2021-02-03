using System.Linq;
using GraphQL.Types;
using GraphQlHelperLib;
using PersonModelLib.Data;
using PersonModelLib.Type;
using RepoInterfaceLib;

namespace PersonModelLib.Query
{
    public class PersonQuery : ObjectGraphType
    {
        public PersonQuery(IRepo<GraphQLDbContext> repo)
        {
            FieldAsync<ListGraphType<PersonType>>("persons", resolve: async context =>
                {
                    var persons = await repo.FetchAsync(dbContext => dbContext.Persons.ToList());
                    context.SetCache<GqlCache>("personIds", persons.Select(p => p.Id).ToList());
                    return persons;
                });
        }
    }
}
