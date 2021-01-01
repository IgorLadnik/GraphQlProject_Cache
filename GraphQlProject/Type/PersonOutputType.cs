using GraphQL.Types;
using GraphQlHelperLib;

namespace GraphQlProject.Type
{
    public class PersonOutputType : ObjectGraphType<MutationResponse>
    {
        public PersonOutputType()
        {
            Field(p => p.Status);
            Field(p => p.Message);
        }
    }
}
