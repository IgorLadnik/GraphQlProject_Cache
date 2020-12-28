using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQlProject.Query
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<PersonQuery>("personQuery", resolve: context => new { });
            Field<OrganizationQuery>("organizationQuery", resolve: context => new { });
        }
    }
}
