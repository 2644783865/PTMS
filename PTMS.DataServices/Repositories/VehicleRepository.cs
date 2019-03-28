using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class VehicleRepository : DataServiceAsync<Vehicle>, IVehicleRepository
    {
        private readonly string[] _includesForPage =
        {
            nameof(Vehicle.Route),
            nameof(Vehicle.Transporter),
            nameof(Vehicle.VehicleType)
        };

        public VehicleRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public Task<PageResult<Vehicle>> FindByParamsForPageAsync(
            int? routeId,
            int? vehicleTypeId,
            int? transporterId,
            int? page,
            int? pageSize)
        {
            Expression<Func<Vehicle, bool>> filter = x => true;

            filter = filter.AndIf(routeId.HasValue, x => x.RouteId == routeId);
            filter = filter.AndIf(vehicleTypeId.HasValue, x => x.VehicleTypeId == vehicleTypeId);
            filter = filter.AndIf(transporterId.HasValue, x => x.TransporterId == transporterId);

            return FindPagedAsync(
                filter,
                x => x.Route.Name,
                true,
                page,
                pageSize,
                _includesForPage);
        }

        public Task<Vehicle> GetByIdForPageAsync(int id)
        {
            return GetByIdAsync(id, _includesForPage);
        }
    }
}
