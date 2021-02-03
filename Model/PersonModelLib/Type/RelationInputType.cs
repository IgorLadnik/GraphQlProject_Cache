using GraphQL.Types;

namespace PersonModelLib.Type
{
    public class RelationInputType : InputObjectGraphType
    {
        public RelationInputType()
        {
            Field<IntGraphType>("Id");
            Field<IntGraphType>("Since");
            Field<StringGraphType>("Kind");
            Field<StringGraphType>("Notes");
            Field<IntGraphType>("P2Id");
        }
    }
}
