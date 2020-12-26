using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQlProject.Interfaces;
using GraphQlProject.Models;

namespace GraphQlProject.Type
{
    public class MenuType : ObjectGraphType<Menu>
    {
        public MenuType(ISubMenu subMenuService)
        {
            const string nameMenus = "menus";
            const string nameSubMenus = "submenus";

            Field(m => m.Id);
            Field(m => m.Name);
            Field(m => m.ImageUrl);
            Field<ListGraphType<SubMenuType>>(nameSubMenus, resolve: context =>
                {
                    var cache = context.GetCache(nameMenus);
                    if (cache.CurrentNumber++ == 0)
                    {
                        // For the 1st time in this level
                        var allMenuIds = (int[])cache.Payload;

                        // Cache all data for the entire level
                        context.SetCache(nameSubMenus, 
                            // Fetch data from database for the entire level
                            new Cache { Payload = subMenuService.GetSubMenus(allMenuIds)?.ToArray() });      
                    }

                    var allCachedSubMenus = (SubMenu[])context.GetCache(nameSubMenus).Payload;
                    var subMenus = allCachedSubMenus?.Where(s => s.MenuId == context.Source.Id);

                    return subMenus;
                });
        }
    }
}
