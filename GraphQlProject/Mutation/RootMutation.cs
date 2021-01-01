using GraphQL.Types;

namespace GraphQlProject.Mutation
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            Field<PersonMutation>("personMutation", resolve: contect => new { });
        }
    }
}
