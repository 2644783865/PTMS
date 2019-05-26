using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class ObjectRepository : DataServiceAsync<Objects>, IObjectRepository
    {
        private readonly string[] _includesFull =
        {
            nameof(Objects.CarBrand),
            nameof(Objects.CarBrand) + "." + nameof(Objects.CarBrand.CarType),
            nameof(Objects.Provider),
            nameof(Objects.Route),
            nameof(Objects.Project),
        };

        private readonly string[] _includesLight =
        {
            nameof(Objects.CarBrand),
            nameof(Objects.Route)
        };

        public ObjectRepository(ApplicationDbContext context)
            :base(context)
        {

        }

        public Task<PageResult<Objects>> FindByParamsAsync(
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? transporterId,
            ModelFormatsEnum format,
            int? page,
            int? pageSize)
        {
            Expression<Func<Objects, bool>> filter = x => (string.IsNullOrEmpty(plateNumber) || x.Name.Contains(plateNumber, StringComparison.InvariantCultureIgnoreCase))
                && (!transporterId.HasValue || x.ProjId == transporterId)
                && (string.IsNullOrEmpty(routeName) || x.Route.Name.Contains(routeName, StringComparison.InvariantCultureIgnoreCase))
                && (!carTypeId.HasValue || x.CarBrand.CarTypeId == carTypeId);

            var includes = _includesFull;

            if (format == ModelFormatsEnum.Light)
            {
                includes = _includesLight;
            }

            return FindPagedAsync(
                filter,
                x => x.Name,
                true,
                page,
                pageSize,
                includes);
        }

        public Task<Objects> GetByIdAsync(decimal id)
        {
            return GetAsync(x => x.Ids == id);
        }

        public Task<Objects> GetFullByIdAsync(decimal id)
        {
            return GetAsync(x => x.Ids == id, _includesFull);
        }
    }
}
