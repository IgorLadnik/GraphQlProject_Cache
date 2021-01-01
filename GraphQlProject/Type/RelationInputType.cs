using GraphQL.Types;

namespace GraphQlProject.Type
{
    public class RelationInputType : InputObjectGraphType
    {
        public RelationInputType()
        {
            Field<IntGraphType>("Id");
            //Field<StringGraphType>("StrId");
            Field<IntGraphType>("Since");
            Field<StringGraphType>("Kind");
            Field<StringGraphType>("Notes");
            //Field<IntGraphType>("P1Id");
            Field<IntGraphType>("P2Id");
        }
    }
}
