using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Models;
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
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            UserAvailableRoutes userRoutesModel,
            int? page,
            int? pageSize)
        {
            var locked = !active;
            
            if (userRoutesModel.ProjectId.HasValue)
            {
                projectId = userRoutesModel.ProjectId;
            }

            Expression<Func<Objects, bool>> filter = x => (string.IsNullOrEmpty(plateNumber) || x.Name.Contains(plateNumber, StringComparison.InvariantCultureIgnoreCase))
                && (!projectId.HasValue || x.ProjId == projectId)
                && (string.IsNullOrEmpty(routeName) || x.Route.Name.Contains(routeName, StringComparison.InvariantCultureIgnoreCase))
                && (!carTypeId.HasValue || x.CarBrand.CarTypeId == carTypeId)
                && (!locked.HasValue || x.ObjOutput == locked.Value)
                && (userRoutesModel.RouteIds == null || (x.LastRout.HasValue && userRoutesModel.RouteIds.Contains(x.LastRout.Value)));

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
