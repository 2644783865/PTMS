using Microsoft.Extensions.DependencyInjection;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Services;
using PTMS.Infrastructure;
using PTMS.Templates;

namespace PTMS.DI
{
    internal static class ConfigureBusinessLogicExtension
    {
        internal static void ConfigureBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IObjectService, ObjectService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<ICarBrandService, CarBrandService>();
            services.AddScoped<ICarTypeService, CarTypeService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IBlockTypeService, BlockTypeService>();
            services.AddScoped<IBusStationService, BusStationService>();
            services.AddScoped<IBusStationRouteService, BusStationRouteService>();
            services.AddScoped<IEventLogService, EventLogService>();

            services.AddScoped<EventLogCreator>();

            services.AddSingleton<IHtmlBuilder, HtmlBuilder>();

            services.AddScoped<IPdfService, PdfService>();
        }
    }
}
