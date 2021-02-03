using GraphQL.Types;
using PersonModelLib.Models;

namespace PersonModelLib.Type
{
    public class RoleType : ObjectGraphType<Role>
            {
        public RoleType()
        {
            Field(r => r.Id);
            Field(r => r.Name);
            Field(r => r.Description);
        }
    }
}


