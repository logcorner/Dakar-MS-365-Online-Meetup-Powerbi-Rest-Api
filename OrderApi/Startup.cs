using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using OrderApi.Repositories;
using System;
using System.Collections.Generic;

namespace OrderApi
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
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Power Bi Rest API - Order HTTP API",
                    Version = "v1",
                    Description = "The Power Bi Rest API  Order HTTP API"
                });
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,

                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://login.microsoftonline.com/49ae1e1a-3f6e-4708-a119-3db19ac9d323/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri("https://login.microsoftonline.com/49ae1e1a-3f6e-4708-a119-3db19ac9d323/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"https://spseventdackar.onmicrosoft.com/api/orders/MyOrders","get order list"}
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new[] {
                                "https://spseventdackar.onmicrosoft.com/api/orders/MyOrders"
                              }
                    }
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
           .UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
               c.OAuthClientId("c0c00309-3d10-49ff-b9dd-d5184af7b613");
               c.OAuthClientSecret("3AxFhi.fsIfiuai9_E4~eoUw3x-0r__7.P");
               c.OAuthAppName("The Speech Micro Service Command Swagger UI");
               c.OAuthScopeSeparator(" ");

               c.OAuthUsePkce();
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