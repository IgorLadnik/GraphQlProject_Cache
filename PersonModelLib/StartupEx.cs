using GraphQlHelperLib;
using Microsoft.Extensions.DependencyInjection;
using PersonModelLib.Data;
using PersonModelLib.Mutation;
using PersonModelLib.Query;
using PersonModelLib.Type;

namespace PersonModelLib
{
    public static class StartupEx
    {
        public static void AddPersonModelServices(this IServiceCollection services, string connectionString) 
        {
            GraphQLDbContext.ConnectionString = connectionString;
            services.AddSingleton<DbProvider<GraphQLDbContext>>();
            services.AddTransient<AffiliationType>();
            services.AddTransient<OrganizationType>();
            services.AddTransient<RelationType>();
            services.AddTransient<PersonType>();
            services.AddTransient<RoleType>();
            services.AddTransient<PersonQuery>();
            services.AddTransient<PersonByIdQuery>();
            services.AddTransient<OrganizationQuery>();
            services.AddTransient<PersonInputType>();
            services.AddTransient<PersonOutputType>();
            services.AddTransient<AffiliationInputType>();
            services.AddTransient<RelationInputType>();
            services.AddTransient<RoleInputType>();
            services.AddTransient<PersonMutation>();
            services.AddTransient<RootQuery>();
            services.AddTransient<RootMutation>();
        }
    }
}
