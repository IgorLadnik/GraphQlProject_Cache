using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQlProject.Query;
using GraphQlProject.Schema;
using GraphQL.Types;
using GraphQL.Server;
using GraphiQl;
using Microsoft.EntityFrameworkCore;
using GraphQlHelperLib;
using GraphQlProject.Mutation;
//using GraphQlProject.Auth;
using JwtHelperLib;
using PersonModelLib;

namespace GraphQlProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJwtAuth(new JwtOptions(Configuration));
            services.AddScoped<AuthService, AuthService>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            //});

            //services.AddDbContext<GraphQLDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //var dbContext = services.BuildServiceProvider(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))).GetService<GraphQLDbContext>();

            services.AddControllers();

            services.AddPersonModelServices(Configuration.GetConnectionString("DefaultConnection"));
            //services.AddGraphQLAuth();
            services.AddTransient<RootQuery>();
            services.AddTransient<RootMutation>();
            services.AddSingleton<ISchema, RootSchema>();

            services.AddGraphQL(options => 
            {
                options.EnableMetrics = false;
            })
            .AddSystemTextJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, GraphQLDbContext dbContext*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //dbContext.Database.EnsureCreated();

            app.UseGraphiQl("/gqli");
            app.UseGraphQL<ISchema>();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //public static class Constants
        //{
        //    public static class Strings
        //    {
        //        public static class JwtClaimIdentifiers
        //        {
        //            public const string Rol = "rol", Id = "id";
        //        }

        //        public static class JwtClaims
        //        {
        //            public const string ApiAccess = "api_access";
        //        }
        //    }
        //}
    }
}
