using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GraphQlProject.Interfaces;
using GraphQlProject.Services;
using GraphQlProject.Type;
using GraphQlProject.Query;
using GraphQlProject.Schema;
using GraphQL.Types;
using GraphQL.Server;
using GraphiQl;
using GraphQlProject.Mutation;
using GraphQlProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            services.AddDbContext<GraphQLDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();

            services.AddTransient<IMenu, MenuService>();
            services.AddTransient<ISubMenu, SubMenuService>();
            services.AddTransient<IReservation, ReservationService>();
            services.AddTransient<MenuType>();
            services.AddTransient<SubMenuType>();
            services.AddTransient<ReservationType>();
            services.AddTransient<MenuQuery>();
            services.AddTransient<SubMenuQuery>();
            services.AddTransient<ReservationQuery>();
            services.AddTransient<RootQuery>();
            services.AddTransient<MenuInputType>();
            services.AddTransient<SubMenuInputType>();
            services.AddTransient<ReservationInputType>();
            services.AddTransient<MenuMutation>();
            services.AddTransient<SubMenuMutation>();
            services.AddTransient<ReservationMutation>();
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
