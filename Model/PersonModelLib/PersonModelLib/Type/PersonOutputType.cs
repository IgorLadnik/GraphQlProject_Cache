using GraphQL.Types;
using RepoInterfaceLib;

namespace PersonModelLib.Type
{
    public class PersonOutputType : ObjectGraphType<RepoResponse>
    {
        public PersonOutputType()
        {
            Field(p => p.Status);
            Field(p => p.Message);
        }
    }
}
