using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQlProject.Interfaces;
using GraphQlProject.Type;

namespace GraphQlProject.Query
{
    public class SubMenuQuery : ObjectGraphType
    {
        public SubMenuQuery(ISubMenu subMenuService)
        {
            Field<ListGraphType<SubMenuType>>("submenus", resolve: context => subMenuService.GetSubMenus());

            Field<ListGraphType<SubMenuType>>("submenuById",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context =>
                {
                    return subMenuService.GetSubMenus(context.GetArgument<int>("id"));
                });
        }
    }
}
