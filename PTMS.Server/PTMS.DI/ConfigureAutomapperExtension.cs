using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.EventLog;
using PTMS.BusinessLogic.Models.Object;
using PTMS.BusinessLogic.Models.User;
using PTMS.Domain.Entities;
using System.Linq;

namespace PTMS.DI
{
    internal static class ConfigureAutomapperExtension
    {
        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Route, RouteModel>();
                CreateMap<Route, RouteFullModel>();

                CreateMap<RouteFullModel, Route>()
                    .ForMember(m => m.BusStationRoutes, options => options.Ignore());

                CreateMap<Objects, ObjectModel>();
                CreateMap<ObjectModel, Objects>()
                    .ForMember(m => m.CarBrand, options => options.Ignore())
                    .ForMember(m => m.Provider, options => options.Ignore())
                    .ForMember(m => m.Route, options => options.Ignore())
                    .ForMember(m => m.Block, options => options.Ignore())
                    .ForMember(m => m.Project, options => options.Ignore());

                CreateMap<Project, ProjectModel>();
                CreateMap<ProjectModel, Project>();

                CreateMap<AppUser, UserModel>()
                    .ForMember(m => m.Status, options => options.MapFrom(x => new UserStatusModel(x.Status)));

                CreateMap<AppRole, RoleModel>();

                CreateMap<BlockType, BlockTypeModel>();
                CreateMap<BlockTypeModel, BlockType>();

                CreateMap<CarType, CarTypeModel>();
                CreateMap<CarTypeModel, CarType>();

                CreateMap<CarBrand, CarBrandModel>();
                CreateMap<CarBrandModel, CarBrand>();

                CreateMap<Provider, ProviderModel>();
                CreateMap<ProviderModel, Provider>();

                CreateMap<Granit, GranitModel>();
                CreateMap<GranitModel, Granit>();

                CreateMap<BusStation, BusStationModel>();
                CreateMap<BusStationModel, BusStation>();

                CreateMap<BusStationRoute, BusStationRouteModel>();
                CreateMap<BusStationRouteModel, BusStationRoute>();

                CreateMap<EventLog, EventLogModel>();
                CreateMap<EventLogField, EventLogFieldModel>();

                CreateMap<AppUser, UserLightModel>()
                    .ForMember(x => x.RoleId, x => x.MapFrom(y => y.UserRoles.First().RoleId))
                    .ForMember(m => m.Status, options => options.MapFrom(x => new UserStatusModel(x.Status)));
            }
        }

        internal static void ConfigureAutomapper(this IServiceCollection services)
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
