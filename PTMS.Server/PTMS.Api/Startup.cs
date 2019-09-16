using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PTMS.Api.Config;
using PTMS.Common;
using PTMS.DI;
using PTMS.Persistance;
using System.Globalization;

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
            services.ConfigureAppCore(Configuration);

            services.ConfigureAuthorization(Configuration);

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

            services.ConfigureCors(Configuration);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidatorActionFilter));
                options.Filters.Add(typeof(NotFoundResultFilter));
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseHsts();
            //    app.UseHttpsRedirection();
            //}

            app.UseMiddleware<TokenFromQueryMiddlewareExtension>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
