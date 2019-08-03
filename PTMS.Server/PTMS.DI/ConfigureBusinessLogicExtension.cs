using Microsoft.Extensions.DependencyInjection;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Services;
using PTMS.Infrastructure;

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
        }
    }
}
