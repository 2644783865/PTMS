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
    public class ObjectRepository : DataServiceAsync<Objects>, IObjectRepository
    {
        private readonly string[] _includesForPage =
        {
            nameof(Objects.CarBrand),
            nameof(Objects.Provider)
        };

        public ObjectRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public Task<PageResult<Objects>> FindByParamsForPageAsync(
            string plateNumber,
            string routeName,
            int? vehicleTypeId,
            int? transporterId,
            int? page,
            int? pageSize)
        {
            Expression<Func<Objects, bool>> filter = x => true;

            filter = filter.AndIf(!string.IsNullOrEmpty(plateNumber), x => x.Name.Contains(plateNumber));
            //filter = filter.AndIf(!string.IsNullOrEmpty(routeName), x => x.Route.Name.Contains(routeName));
            //filter = filter.AndIf(vehicleTypeId.HasValue, x => x.VehicleTypeId == vehicleTypeId);
            //filter = filter.AndIf(transporterId.HasValue, x => x.TransporterId == transporterId);

            return FindPagedAsync(
                filter,
                x => x.Name,
                true,
                page,
                pageSize,
                _includesForPage);
        }

        public Task<Objects> GetByIdForPageAsync(int id)
        {
            return GetByIdAsync(id, _includesForPage);
        }
    }
}
