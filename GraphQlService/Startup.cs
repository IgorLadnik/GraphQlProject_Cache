using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GraphQlService.Schema;
using GraphQL.Types;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphiQl;
using PersonModelLib;
using JwtAuthLib;

namespace GraphQlService
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
            services.AddPersonModelServices(Configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton<ISchema, RootSchema>();

            if (Configuration.GetValue<bool>("FeatureToggles:IsAuthJwt"))
                services.AddJwtAuth(new JwtOptions(Configuration));

            services.AddControllers();

            services.AddGraphQL(options => 
            {
                options.EnableMetrics = false;
            })
            .AddSystemTextJson();

            if (Configuration.GetValue<bool>("FeatureToggles:IsOpenApiSwagger"))
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
                            Array.Empty<string>()
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

            if (Configuration.GetValue<bool>("FeatureToggles:IsOpenApiSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                    {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphQL API v1");
                        options.DefaultModelsExpandDepth(-1);
                    });
            }

            if (Configuration.GetValue<bool>("FeatureToggles:IsGraphQLSchema"))
                app.UseGraphQL<ISchema>("/graphql");

            if (Configuration.GetValue<bool>("FeatureToggles:IsGraphIql"))
                app.UseGraphiQl("/graphIql", "/graphql");

            if (Configuration.GetValue<bool>("FeatureToggles:IsGraphQLPlayground"))
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
                {
                    GraphQLEndPoint = "/graphql",
                    Path = "/playground",
                });

            app.UseHttpsRedirection();
            app.UseRouting();

            if (Configuration.GetValue<bool>("FeatureToggles:IsAuthJwt"))
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
