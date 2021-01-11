using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
using System.Reflection;
using System.IO;
using System;

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

            services.AddControllers();

            services.AddTransient<RootQuery>();
            services.AddTransient<RootMutation>();
            services.AddSingleton<ISchema, RootSchema>();

            services.AddGraphQL(options => 
            {
                options.EnableMetrics = false;
            })
            .AddSystemTextJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GraphQL API",
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, GraphQLDbContext dbContext*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //dbContext.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphQL API v1"));

            app.UseGraphQL<ISchema>("/graphql");

            app.UseGraphiQl("/gqli");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                GraphQLEndPoint = "/graphql",
                Path = "/playground",
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
