using GraphQL.Types;
using PersonModelLib.Query;

namespace PersonModelLib.Query
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<PersonQuery>("personQuery", resolve: context => new { });
            Field<PersonByIdQuery>("personByIdQuery", resolve: context => new { });
            Field<OrganizationQuery>("organizationQuery", resolve: context => new { });
        }
    }
}
