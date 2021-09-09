using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services
               .AddAuthentication(options =>
               {
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
               }
               ).AddJwtBearer(options => Configuration.Bind("AzureAd", options));

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
                            AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{Configuration["SwaggerUI:TenantId"]}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri($"https://login.microsoftonline.com/{Configuration["SwaggerUI:TenantId"]}/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { $"https://{Configuration["SwaggerUI:Domain"]}.onmicrosoft.com/api/orders/my-orders","get order list"}
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
                               $"https://{Configuration["SwaggerUI:Domain"]}.onmicrosoft.com/api/orders/my-orders"
                              }
                    }
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
           .UseSwaggerUI(c =>
           {
               var oAuthClientId = Configuration["SwaggerUI:OAuthClientId"];
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
               c.OAuthClientId(oAuthClientId);
               c.OAuthAppName("The Power Bi Rest API  Order Swagger UI");
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