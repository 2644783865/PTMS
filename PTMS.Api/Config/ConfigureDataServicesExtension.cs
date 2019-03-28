using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Repositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.Api.Config
{
    public static class ConfigureDataServicesExtension
    {
        public static void ConfigureDataServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ITransporterRepository, TransporterRepository>();
        }
    }
}
