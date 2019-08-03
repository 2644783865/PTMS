using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTMS.Common;
using PTMS.Persistance;

namespace PTMS.DI
{
    public static class ConfigureAppExtension
    {
        public static void ConfigureAppCore(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseFirebird(Configuration.GetSection("AppSettings").Get<AppSettings>().ProjectsDatabaseConnection);
            });

            services.ConfigureAutomapper();
            services.ConfigureBusinessLogic();
            services.ConfigureDataServices();
        }
    }
}
