using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using GraphQlProject.Models;
using GraphQlProject.Type;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace GraphQlProject.Subscription
{
    public class PersonSubscription : ObjectGraphType 
    {
        private readonly IPerson _person;

        public PersonSubscription(IPerson personDetails)
        {
            _person = personDetails;

            AddField(new EventStreamFieldType
            {
                Name = "personAdded",
                Type = typeof(MessageType),
                Resolver = new FuncFieldResolver<Message>(ResolvePerson),
                Subscriber = new EventStreamResolver<Message>(Subscribe)
            });
        }

        private Message ResolvePerson(IResolveFieldContext context)
        {
            return context.Source as Message;
        }

        private IObservable<Message> Subscribe(IResolveEventStreamContext context)
        {
            return _person.GetLatestPersons();
        }
    }

    public interface IPerson
    {
        IObservable<Message> GetLatestPersons();
        Message AddPerson(Message personDetails);
        ConcurrentStack<Message> AllPersons { get; }
    }

    public class PersonDetails : IPerson
    {
        private readonly ISubject<Message> _messageStream = new ReplaySubject<Message>(1);
        public ConcurrentStack<Message> AllPersons { get; }

        public PersonDetails()
        {
            AllPersons = new ConcurrentStack<Message>();
        }

        public IObservable<Message> GetLatestPersons()
        {
            return _messageStream.Select(message => message).AsObservable();
            //AllPersons.ToObservable();
        }

        public Message AddPerson(Message message)
        {
            AllPersons.Push(message);
            _messageStream.OnNext(message);
            return message;
        }

        public void AddError(Exception exception)
        {
            _messageStream.OnError(exception);
        }
    }
}

/*
subscription {
  personAdded {
    id
    name
  }
}
*/