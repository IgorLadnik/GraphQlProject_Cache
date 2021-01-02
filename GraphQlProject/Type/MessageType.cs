using GraphQL.Types;
using GraphQlProject.Models;

namespace GraphQlProject.Type
{
    public class MessageType : ObjectGraphType<Message>
    {
        public MessageType()
        {
            Field(o => o.Id);

            Field(o => o.Name);
        }

        //public MessageType()
        //{
        //    Field(o => o.Content);
        //    Field(o => o.SentAt);
        //    //Field(o => o.From, false, typeof(MessageFromType)).Resolve(ResolveFrom);
        //}

        ////private MessageFrom ResolveFrom(IResolveFieldContext<Message> context)
        ////{
        ////    var message = context.Source;
        ////    return message.From;
        ////}
    }
}
