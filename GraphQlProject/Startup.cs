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

            app.UseGraphiQl("/graphql");
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
