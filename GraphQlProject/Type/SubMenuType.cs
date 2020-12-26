using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQlProject.Models;

namespace GraphQlProject.Type
{
    public class SubMenuType : ObjectGraphType<SubMenu>
    {
        public SubMenuType()
        {
            Field(s => s.Id);
            Field(s => s.Name);
            Field(s => s.Description);
            Field(s => s.ImageUrl);
            Field(s => s.Price);
            Field(s => s.MenuId);
        }
    }
}
