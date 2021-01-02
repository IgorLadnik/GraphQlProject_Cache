﻿using System;
using Microsoft.Extensions.DependencyInjection;
using GraphQlProject.Query;
using GraphQlProject.Mutation;
using GraphQlProject.Subscription;

namespace GraphQlProject.Schema
{
    public class RootSchema : GraphQL.Types.Schema
    {
        public RootSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<RootQuery>();
            Mutation = serviceProvider.GetRequiredService<RootMutation>();
            Subscription = serviceProvider.GetRequiredService<PersonSubscription>();
        }
    }
}
