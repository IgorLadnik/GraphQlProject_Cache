using GraphQL.Types;
using PersonModelLib.Models;

namespace PersonModelLib.Type
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Field(u => u.Id);
            //Field(u => u.StrId);
            Field(u => u.UserName);
            Field(u => u.Password);
            Field(u => u.Type);
        }
    }
}
