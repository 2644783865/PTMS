using AutoMapper;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.User;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Routs, RouteModel>();
            CreateMap<RouteModel, Routs>();

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
        }
    }
}
