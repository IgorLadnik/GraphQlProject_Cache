using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Models;

namespace GraphQlProject.Interfaces
{
    public interface IMenu
    {
        IEnumerable<Menu> GetMenus();
        Menu AddMenu(Menu menu);
    }
}
