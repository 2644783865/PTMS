using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTMS.Common;

namespace PTMS.Api.Config
{
    public static class ConfigureCorsExtension
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration Configuration)
        {
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(appSettings.CorsAllowedOrigins);
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
        }
    }
}
