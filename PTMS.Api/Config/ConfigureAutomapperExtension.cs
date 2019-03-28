using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTMS.BusinessLogic.Config;

namespace PTMS.Api.Config
{
    public static class ConfigureAutomapperExtension
    {
        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
