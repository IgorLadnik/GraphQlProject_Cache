using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.Validation;
using GraphQL.Authorization;
using IAuthorizationEvaluator = GraphQL.Authorization.IAuthorizationEvaluator;
using System.Security.Claims;

namespace GraphQlProject.Auth
{
    public static class ServiceConfigurationEx
    {
        public static void AddGraphQLAuth(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.AddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();

                authSettings.AddPolicy("A", _ => _.RequireClaim(ClaimTypes.Role, "aaa"));
                authSettings.AddPolicy("B", _ => _.RequireClaim(ClaimTypes.Role, "bbb"));
                authSettings.AddPolicy("C", _ => _.RequireClaim(ClaimTypes.Role, "ccc"));

                return authSettings;
            });
        }
    }
}
