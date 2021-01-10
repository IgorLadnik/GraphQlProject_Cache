using GraphQL.Types;

namespace PersonModelLib.Type
{
    public class RoleInputType : InputObjectGraphType
    {
        public RoleInputType()
        {
            Field<IntGraphType>("Id");
            //Field<StringGraphType>("StrId");
            Field<StringGraphType>("Name");
            Field<StringGraphType>("Description");
        }
    }
}
