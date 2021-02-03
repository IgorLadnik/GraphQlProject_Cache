using GraphQL.Types;

namespace PersonModelLib.Type
{
    public class PersonInputType : InputObjectGraphType
    {
        public PersonInputType()
        {
            Field<IntGraphType>("Id");
            Field<StringGraphType>("GivenName");
            Field<StringGraphType>("Surname");
            Field<IntGraphType>("Born");
            Field<StringGraphType>("Phone");
            Field<StringGraphType>("Email");
            Field<StringGraphType>("Address");
            Field<ListGraphType<AffiliationInputType>>("Affiliations");
            Field<ListGraphType<RelationInputType>>("Relations");
        }
    }
}
