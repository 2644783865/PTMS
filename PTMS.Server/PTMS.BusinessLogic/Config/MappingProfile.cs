using AutoMapper;
using PTMS.BusinessLogic.Models;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Route, RouteModel>();
            CreateMap<RouteModel, Route>()
                .ForMember(m => m.Vehicles, options => options.Ignore());

            CreateMap<Vehicle, VehicleModel>();
            CreateMap<VehicleModel, Vehicle>()
                .ForMember(m => m.Route, options => options.Ignore())
                .ForMember(m => m.VehicleType, options => options.Ignore())
                .ForMember(m => m.Transporter, options => options.Ignore());

            CreateMap<VehicleType, VehicleTypeModel>();
            CreateMap<VehicleTypeModel, VehicleType>();

            CreateMap<Transporter, TransporterModel>();
            CreateMap<TransporterModel, Transporter>()
                .ForMember(m => m.Vehicles, options => options.Ignore());

            CreateMap<User, UserModel>();
        }
    }
}
