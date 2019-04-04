using Microsoft.Extensions.DependencyInjection;

namespace PTMS.Api.Config
{
    public static class ConfigureCorsExtension
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200");
                    builder.WithHeaders("Content-Type", "Authorization");
                });
            });
        }
    }
}
