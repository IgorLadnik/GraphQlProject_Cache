using GraphQL.Types;

namespace GraphQlProject.Type
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
