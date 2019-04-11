using Microsoft.Extensions.DependencyInjection;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Services;

namespace PTMS.Api.Config
{
    public static class ConfigureBusinessLogicExtension
    {
        public static void ConfigureBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITransporterService, TransporterService>();
            services.AddScoped<IVehicleTypeService, VehicleTypeService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
