using Microsoft.Extensions.DependencyInjection;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Repositories;

namespace PTMS.Api.Config
{
    public static class ConfigureDataServicesExtension
    {
        public static void ConfigureDataServices(this IServiceCollection services)
        {
            services.AddScoped<IDbTransactionHelper, DbTransactionHelper>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IObjectRepository, ObjectRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<ICarBrandRepository, CarBrandRepository>();
            services.AddScoped<ICarTypeRepository, CarTypeRepository>();
        }
    }
}
