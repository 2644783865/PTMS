using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PTMS.BusinessLogic.Config;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.Api.Config
{
    public static class ConfigureAuthorizationExtension
    {
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // ===== Add Jwt Authentication ========
            var jwtConfig = Configuration.GetSection("AppSettings").Get<AppSettings>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtConfig.JwtIssuer,
                        ValidAudience = jwtConfig.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtKey)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
        }
    }
}
