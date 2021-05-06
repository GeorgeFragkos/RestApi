using API.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Services;
using System.Collections.Generic;
using API.Authorization;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.AspNetCore;
using API.Filters;

namespace API.Installer
{
    public class MVCInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddControllersWithViews();
               
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            })
                .AddFluentValidation(mvcConfiguration => mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);


            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
            

            services.AddSingleton(TokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
               AddJwtBearer(x =>
               {
                   x.SaveToken = true;
                   x.TokenValidationParameters = TokenValidationParameters;
               });

            services.AddAuthorization(options => 
            {
                //Adding Claim code 
                //options.AddPolicy("TagViewer", builder => builder.RequireClaim("tags.view", "true"));

                //Custom Requirment with custom Policy
                options.AddPolicy("MustWorkForGeorge", policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirment("george.com"));
                });
            });

            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
                x.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                var security = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", 
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {new OpenApiSecurityScheme{Reference = new OpenApiReference{Id = "Bearer",Type=ReferenceType.SecurityScheme}}
                            ,new List<string>()
                    } });
            });
        }
    }
}
