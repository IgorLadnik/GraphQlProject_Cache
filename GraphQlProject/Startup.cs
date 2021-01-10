using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using GraphQlProject.Query;
using GraphQlProject.Schema;
using GraphQL.Types;
using GraphQL.Server;
using GraphiQl;
using GraphQlProject.Mutation;
//using GraphQlProject.Auth;
using JwtHelperLib;
using PersonModelLib;
using GraphQL.Server.Ui.Playground;

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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            services.AddJwtAuth(new JwtOptions(Configuration), connectionString);
            services.AddPersonModelServices(connectionString);
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

            app.UseGraphQL<ISchema>();
            app.UseGraphiQl("/gqli");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions 
            {
                GraphQLEndPoint = "/graphql",
                Path = "/playground"
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
