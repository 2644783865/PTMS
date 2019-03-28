using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTMS.Api.Config;
using PTMS.Persistance;

namespace PTMS.Api
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.ConfigureAuthorization(Configuration);
            services.ConfigureDataServices();
            services.ConfigureBusinessLogic();
            services.ConfigureAutomapper();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var defaultCulture = new CultureInfo("ru-RU");

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);

                options.SupportedCultures.Clear();
                options.SupportedCultures.Add(defaultCulture);

                options.SupportedUICultures.Clear();
                options.SupportedUICultures.Add(defaultCulture);

                options.RequestCultureProviders = null;
            });            

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidatorActionFilter));
                options.Filters.Add(typeof(NotFoundResultFilter));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.ConfigureExceptionHandler();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
