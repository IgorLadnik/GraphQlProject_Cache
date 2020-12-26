using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Models;

namespace GraphQlProject.Interfaces
{
    public interface ISubMenu
    {
        IEnumerable<SubMenu> GetSubMenus();
        IEnumerable<SubMenu> GetSubMenus(params int[] menuIds);
        SubMenu AddSubMenu(SubMenu subMenu);
    }
}
