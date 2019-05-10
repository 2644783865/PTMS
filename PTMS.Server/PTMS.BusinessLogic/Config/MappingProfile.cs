using AutoMapper;
using PTMS.BusinessLogic.Models;
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
                .ForMember(m => m.Provider, options => options.Ignore());

            CreateMap<Project, ProjectModel>();
            CreateMap<ProjectModel, Project>();

            CreateMap<AppUser, UserModel>();
        }
    }
}
