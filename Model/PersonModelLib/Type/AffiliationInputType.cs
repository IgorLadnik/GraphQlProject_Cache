using System;
using GraphQL.Types;

namespace PersonModelLib.Type
{
    public class AffiliationInputType : InputObjectGraphType
    {
        public AffiliationInputType()
        {
            Field<IntGraphType>("Id");
            //Field<StringGraphType>("StrId");
            Field<IntGraphType>("Since");
            Field<IntGraphType>("OrganizationId");
            Field<IntGraphType>("RoleId");
        }
    }
}
