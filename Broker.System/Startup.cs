using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Broker.System.Data;
using Broker.System.Installers;
using Broker.System.Installers.CustomMiddleware;
using Broker.System.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Broker.System
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
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.InstallServices(Configuration);
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMiddleware<TokenExpiredMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseCors("mata");
            app.UseAuthentication();
            app.Use(async (context, next) =>
            {
                await next();
                var bearerAuth = context.Request.Headers["Authorization"]
                    .FirstOrDefault()?.StartsWith("Bearer ") ?? false;
                if (context.Response.StatusCode == 401
                    && !context.User.Identity.IsAuthenticated
                    && !bearerAuth)
                {
                    await context.ChallengeAsync("oidc");
                }
            });
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, "My API V1");

                options.OAuthClientId("demo_api_swagger");
                options.OAuthAppName("Demo API - Swagger");
                options.OAuthUsePkce();
            });
        }
    }
}