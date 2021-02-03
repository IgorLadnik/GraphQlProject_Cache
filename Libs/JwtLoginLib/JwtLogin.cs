using Microsoft.Extensions.DependencyInjection;
using RepoInterfaceLib;
using RepoLib;
using JwtLoginLib.Data;
using JwtAuthLib;

namespace JwtLoginLib
{
    public static class JwtLogin
    {
        public static void AddJwtLogin(this IServiceCollection services, JwtOptions jwtOptions, string connectionString)
        {
            UserDbContext.ConnectionString = connectionString;
            services.AddSingleton<IRepo<UserDbContext>, Repo<UserDbContext>>();
            services.AddScoped<AuthenticationService, AuthenticationService>();

            JwtAuth.AddJwtAuth(services, jwtOptions);
        }
    }
}
