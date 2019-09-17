using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using PTMS.Templates.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PTMS.Templates
{
    public class HtmlBuilder : IHtmlBuilder
    {
        private readonly RazorViewToHtmlRenderer _razorViewToHtmlRenderer;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public HtmlBuilder(
            ILogger<HtmlBuilder> logger,
            ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;

            _razorViewToHtmlRenderer = InitializeServices();
        }

        public async Task<string> GetObjectsTable(ObjectsPrintModel model)
        {
            var result = await GetHtml("ObjectsPrint", model);
            return result;
        }

        #region Private
        private async Task<string> GetHtml<T>(string viewName, T model)
        {
            var startDate = DateTime.Now;
            var result = await _razorViewToHtmlRenderer.RenderViewToHtmlString(viewName, model);
            var endDate = DateTime.Now;

            _logger.LogWarning("GetHtml method takes {0}ms", (endDate - startDate).Milliseconds);
            return result;
        }

        private RazorViewToHtmlRenderer InitializeServices()
        {
            var services = new ServiceCollection();

            var entryAssembly = Assembly.GetEntryAssembly();

            var applicationName = entryAssembly.GetName().Name;
            IFileProvider fileProvider = new PhysicalFileProvider(Path.GetDirectoryName(entryAssembly.Location));

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = applicationName,
                WebRootFileProvider = fileProvider,
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(fileProvider);

                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/Templates/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/Templates/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);

            services.AddSingleton(_logger);
            services.AddSingleton(_loggerFactory);

            services.AddMvc();
            services.AddSingleton<RazorViewToHtmlRenderer>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetRequiredService<RazorViewToHtmlRenderer>();
        }
        #endregion
    }
}