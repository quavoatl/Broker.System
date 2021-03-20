using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Broker.System.Contracts.V1;
using Broker.System.Options;
using Broker.System.Services;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
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
            services.AddHttpClient();
            services.AddControllers();
            
            // services.AddCors(options =>
            // {
            //     options.AddPolicy(name: "mata",
            //         builder =>
            //         {
            //             builder.WithOrigins("*")
            //                 .AllowAnyHeader()
            //                 .AllowAnyMethod();
            //             ;
            //         });
            // });

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            
            var swaggerOptions = new SwaggerOptions();
            configuration.Bind(nameof(swaggerOptions), swaggerOptions);
            services.AddSingleton(swaggerOptions);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
           
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookies";
                    config.DefaultChallengeScheme = "oidc";
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5005";
                    options.Audience = "broker_limits_rest_client";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = "https://localhost:5005";
                    config.ClientId = "broker_limits_rest_client";
                    config.ClientSecret = "secret";
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    config.GetClaimsFromUserInfoEndpoint = true;

                    config.Scope.Add(ClaimsHelpers.ROLES_KEY);
                    config.ClaimActions.MapUniqueJsonKey(ClaimsHelpers.ROLE, ClaimsHelpers.ROLE, ClaimsHelpers.ROLE);
                    config.TokenValidationParameters.RoleClaimType = ClaimsHelpers.ROLE;
                });
            
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    OpenIdConnectUrl = new Uri($"https://localhost:5005/.well-known/openid-configuration"),
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:5005/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:5005/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"ApiOne", "Api One Resource - mare secret"},
                            }
                        }
                    }
                });

                options.OperationFilter<SwaggerAuthenticationRequirementsOperationFilter>();
            });
        }
    }
}