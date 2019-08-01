using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Models;
using PTMS.DataServices.SyncServices;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class ObjectRepository : DataServiceAsyncEx<Objects, decimal>, IObjectRepository
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
            nameof(Objects.CarBrand) + "." + nameof(Objects.CarBrand.CarType),
            nameof(Objects.Route)
        };

        private readonly string[] _includesPure =
        {
            nameof(Objects.Route)
        };

        private readonly IRouteRepository _routeRepository;

        public ObjectRepository(
            IRouteRepository routeRepository,
            ApplicationDbContext context,
            ObjectsSyncService syncService)
            :base(context, syncService)
        {
            _routeRepository = routeRepository;
        }

        public Task<PageResult<Objects>> FindByParamsAsync(
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            UserAvailableRoutes userRoutesModel,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string sortBy,
            OrderByEnum orderBy,
            int? page,
            int? pageSize)
        {
            var locked = !active;
            
            if (userRoutesModel.ProjectId.HasValue)
            {
                projectId = userRoutesModel.ProjectId;
            }

            if (!string.IsNullOrEmpty(plateNumber))
            {
                plateNumber = plateNumber.ToUpper();
            }

            if (!string.IsNullOrEmpty(routeName))
            {
                routeName = routeName.ToUpper();
            }

            Expression<Func<Objects, bool>> filter = x => (string.IsNullOrEmpty(plateNumber) || x.Name.Contains(plateNumber))
                && (!projectId.HasValue || x.ProjId == projectId)
                && (!carBrandId.HasValue || x.CarBrandId == carBrandId)
                && (!providerId.HasValue || x.ProviderId == providerId)
                && (string.IsNullOrEmpty(routeName) || x.Route.Name.Equals(routeName))
                && (!carTypeId.HasValue || x.CarBrand.CarTypeId == carTypeId)
                && (!locked.HasValue || x.ObjOutput == locked.Value)
                && (!yearRelease.HasValue || x.YearRelease == yearRelease)
                && (userRoutesModel.RouteIds == null || (x.LastRout.HasValue && userRoutesModel.RouteIds.Contains(x.LastRout.Value)));

            var includes = _includesPure;

            if (format == ModelFormatsEnum.Light)
            {
                includes = _includesLight;
            }
            else if (format == ModelFormatsEnum.Full)
            {
                includes = _includesFull;
            }

            Expression<Func<Objects, object>> sortByFilter = null;

            switch (sortBy.ToLower())
            {
                case "name":
                    sortByFilter = x => x.Name;
                    break;
                case "laststationtime":
                    sortByFilter = x => x.LastStationTime;
                    break;
                case "yearrelease":
                    sortByFilter = x => x.YearRelease;
                    break;
                default:
                    sortByFilter = x => x.LastTime;
                    break;
            }

            return FindPagedAsync(
                filter,
                sortByFilter,
                orderBy,
                page,
                pageSize,
                includes);
        }

        public Task<Objects> GetFullByIdAsync(decimal id)
        {
            return GetAsync(x => x.Id == id, _includesFull);
        }

        public Task<List<Objects>> FindForReporting(DateTime onlineStartDate, DateTime onlineEndDate)
        {
            return FindAsync(x => !x.ObjOutput
                && x.LastStationTime.HasValue
                && x.LastStationTime.Value >= onlineStartDate
                && x.LastStationTime.Value <= onlineEndDate
                && x.Route.RouteActive);
        }
    }
}
