using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQlProject.Interfaces;
using GraphQlProject.Type;
using GraphQL.Language.AST;

namespace GraphQlProject.Query
{
    public class MenuQuery : ObjectGraphType
    {
        public MenuQuery(IMenu menuService)
        {
            const string nameMenus = "menus";

            Field<ListGraphType<MenuType>>(nameMenus, resolve: context => 
                {
                    var menus = menuService.GetMenus();
                    context.SetCache(nameMenus, new Cache { Payload = menus.Select(m => m.Id).ToArray() });
                    return menus;
                });
        }
    }
}
