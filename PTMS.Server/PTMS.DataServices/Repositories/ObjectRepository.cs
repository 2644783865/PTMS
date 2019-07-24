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

        public ObjectRepository(
            ApplicationDbContext context,
            ObjectsSyncService syncService)
            :base(context, syncService)
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
            int? carBrandId,
            int? providerId,
            int? yearRelease,
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
                && (!carBrandId.HasValue || x.CarBrandId == carBrandId)
                && (!providerId.HasValue || x.ProviderId == providerId)
                && (string.IsNullOrEmpty(routeName) || x.Route.Name.Equals(routeName, StringComparison.InvariantCultureIgnoreCase))
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

            return FindPagedAsync(
                filter,
                x => x.Name,
                true,
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
                && x.LastTime.HasValue
                && x.LastTime.Value >= onlineStartDate
                && x.LastTime.Value <= onlineEndDate
                && x.Route.RouteActive);
        }
    }
}
