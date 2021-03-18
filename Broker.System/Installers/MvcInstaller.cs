using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Broker.System.Options;
using Broker.System.Services;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Broker.System.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "mata",
                    builder =>
                    {
                        builder.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        ;
                    });
            });

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                //ValidateIssuer = true,
               //ValidateAudience = true,
                ValidAudience = jwtSettings.ValidAudience,
                ValidIssuer = jwtSettings.ValidIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            };
            
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    config.DefaultScheme = "Cookies";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://localhost:5005";
                    config.ClientId = "demo_api_swagger";
                    config.ClientSecret = "secret";
                    config.SaveTokens = true;

                    config.ResponseType = "code";
                });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5005";
                    options.RequireHttpsMetadata = false;
                    // // name of the API resource
                     options.Audience = "api1";
                });
            
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            var req = new OpenApiSecurityRequirement()
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
                        In = ParameterLocation.Cookie,
                    },
                    new List<string>()
                }
            };
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Protected API", Version = "v1"});
    
                // we're going to be adding more here...
                
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:5005/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:5005/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"api1", "Demo API - full access"}
                            }
                        }
                    }
                });
                
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }
    }
}