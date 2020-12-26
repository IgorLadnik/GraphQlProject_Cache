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
            Field<MenuQuery>("menuQuery", resolve: contect => new { });
            Field<SubMenuQuery>("subMenuQuery", resolve: contect => new { });
            Field<ReservationQuery>("reservationQuery", resolve: contect => new { });
        }
    }
}
