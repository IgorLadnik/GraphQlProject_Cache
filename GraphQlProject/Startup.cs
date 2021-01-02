using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQlProject.Type;
using GraphQlProject.Query;
using GraphQlProject.Schema;
using GraphQL.Types;
using GraphQL.Server;
using GraphiQl;
using GraphQlProject.Data;
using Microsoft.EntityFrameworkCore;
using GraphQlHelperLib;
using GraphQlProject.Mutation;
using GraphQlProject.Subscription;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            //services.AddDbContext<GraphQLDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //var dbContext = services.BuildServiceProvider(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))).GetService<GraphQLDbContext>();

            GraphQLDbContext.ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddControllers();
            services.AddSingleton<DbProvider<GraphQLDbContext>>();
            services.AddTransient<AffiliationType>();
            services.AddTransient<OrganizationType>();
            services.AddTransient<RelationType>();
            services.AddTransient<PersonType>();
            services.AddTransient<RoleType>();
            services.AddTransient<UserType>();
            services.AddTransient<PersonQuery>();
            services.AddTransient<PersonByIdQuery>();
            services.AddTransient<OrganizationQuery>();
            services.AddTransient<RootQuery>();
            services.AddTransient<PersonInputType>();
            services.AddTransient<PersonOutputType>();
            services.AddTransient<AffiliationInputType>();
            services.AddTransient<RelationInputType>();
            services.AddTransient<RoleInputType>();
            services.AddTransient<PersonMutation>();
            services.AddTransient<RootMutation>();
            services.AddTransient<MessageType>();
            services.AddTransient<PersonSubscription>();
            services.AddSingleton<IPerson, PersonDetails>();
            //services.AddTransient<RootSubscription>();
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

            app.UseWebSockets();

            app.UseGraphiQl("/gqli");
            app.UseGraphQL<ISchema>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
