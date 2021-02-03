using GraphQL.Types;
using PersonModelLib.Mutation;

namespace PersonModelLib.Mutation
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            Field<PersonMutation>("personMutation", resolve: contect => new { });
        }
    }
}
