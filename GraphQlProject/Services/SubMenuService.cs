using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Data;
using GraphQlProject.Interfaces;
using GraphQlProject.Models;

namespace GraphQlProject.Services
{
    public class SubMenuService : ISubMenu
    {
        private readonly GraphQLDbContext _dbContext;

        public SubMenuService(GraphQLDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SubMenu AddSubMenu(SubMenu subMenu)
        {
            _dbContext.SubMenus.Add(subMenu);
            _dbContext.SaveChanges();
            return subMenu;
        }

        public IEnumerable<SubMenu> GetSubMenus() => _dbContext.SubMenus;
        
        public IEnumerable<SubMenu> GetSubMenus(params int[] menuIds) =>
            GetSubMenus().Where(s => menuIds.Contains(s.MenuId));
    }
}
