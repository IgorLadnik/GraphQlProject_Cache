using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Data;
using GraphQlProject.Interfaces;
using GraphQlProject.Models;

namespace GraphQlProject.Services
{
    public class MenuService : IMenu
    {
        private readonly GraphQLDbContext _dbContext;

        public MenuService(GraphQLDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Menu AddMenu(Menu menu)
        {
            _dbContext.Menus.Add(menu);
            _dbContext.SaveChanges();
            return menu;
        }

        public IEnumerable<Menu> GetMenus() => _dbContext.Menus;
    }
}
