using Microsoft.Extensions.DependencyInjection;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Repositories;
using PTMS.DataServices.SyncServices;

namespace PTMS.Api.Config
{
    public static class ConfigureDataServicesExtension
    {
        public static void ConfigureDataServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IObjectRepository, ObjectRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<ICarBrandRepository, CarBrandRepository>();
            services.AddScoped<ICarTypeRepository, CarTypeRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IProjectRouteRepository, ProjectRouteRepository>();
            services.AddScoped<IFactOfObjectRouteRepository, FactOfObjectRouteRepository>();
            services.AddScoped<IBlockTypeRepository, BlockTypeRepository>();
            services.AddScoped<IGranitRepository, GranitRepository>();

            services.AddScoped<ObjectsSyncService>();
        }
    }
}
