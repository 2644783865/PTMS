using Microsoft.EntityFrameworkCore;
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
    public class ObjectRepository : DataServiceAsync<Objects>, IObjectRepository
    {
        private readonly string[] _includesFull =
        {
            nameof(Objects.CarBrand),
            nameof(Objects.CarBrand) + "." + nameof(Objects.CarBrand.CarType),
            nameof(Objects.Provider),
            nameof(Objects.Route),
            nameof(Objects.Block)
        };

        private readonly string[] _includesLight =
        {
            nameof(Objects.CarBrand),
            nameof(Objects.CarBrand) + "." + nameof(Objects.CarBrand.CarType),
            nameof(Objects.Route),
            nameof(Objects.Block)
        };

        private readonly string[] _includesPure =
        {
            nameof(Objects.Block)
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
            string blockNumber,
            int? blockTypeId,
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
                && (string.IsNullOrEmpty(blockNumber) || x.Phone.ToString().Contains(blockNumber) || x.Block.BlockNumber.ToString().Contains(blockNumber))
                && (!blockTypeId.HasValue || x.Block.BlockTypeId == blockTypeId)
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
                case "transporter":
                    sortByFilter = x => x.Project.Name;
                    break;
                case "route":
                    sortByFilter = x => x.Route.Name;
                    break;
                case "carbrand":
                    sortByFilter = x => x.CarBrand.Name;
                    break;
                case "cartype":
                    sortByFilter = x => x.CarBrand.CarType.Name;
                    break;
                case "provider":
                    sortByFilter = x => x.Provider.Name;
                    break;
                case "phone":
                    sortByFilter = x => x.Phone;
                    break;
                case "status":
                    sortByFilter = x => x.ObjOutput ? 1 : 0;
                    break;
                case "yearrelease":
                    sortByFilter = x => x.YearRelease;
                    break;
                case "block":
                    sortByFilter = x => x.Block.BlockType.Name;
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

        public Task<Objects> GetFullByIdAsync(int id)
        {
            return GetByIdAsync(id, _includesFull);
        }

        public Task<Objects> GetPureByIdAsync(int id)
        {
            return GetByIdAsync(id, _includesPure);
        }

        public Task<Objects> GetByIdWithBlockAsync(int id)
        {
            return GetByIdAsync(id, nameof(Objects.Block));
        }

        public Task<List<Objects>> FindForReportingAsync(DateTime onlineStartDate, DateTime onlineEndDate)
        {
            return FindAsync(x => !x.ObjOutput
                && x.LastStationTime.HasValue
                && x.LastStationTime.Value >= onlineStartDate
                && x.LastStationTime.Value <= onlineEndDate
                && x.Route.RouteActive);
        }

        public override async Task<Objects> AddAsync(Objects entity)
        {
            PrepareObject(entity);
            return await base.AddAsync(entity);
        }

        public override async Task<Objects> UpdateAsync(Objects entity)
        {
            PrepareObject(entity);
            return await base.UpdateAsync(entity);
        }

        public async Task<bool> AnyByPlateNumberAsync(string name, int? currentEntityId)
        {
            name = name.ToUpper();

            var vehicle = await GetAsync(x => x.Name == name 
                && (!currentEntityId.HasValue || x.Id != currentEntityId));

            return vehicle != null;
        }

        public async Task<bool> AnyByPhoneAsync(long phone, int? currentEntityId)
        {
            var vehicle = await GetAsync(x => x.Phone == phone
                && (!currentEntityId.HasValue || x.Id != currentEntityId));

            return vehicle != null;
        }

        public async Task<short> GetNextObjectIdAsync()
        {
            var maxObjId = await EntityQuery.MaxAsync(x => x.ObjId);
            return (short)(maxObjId + 1);
        }

        private void PrepareObject(Objects entity)
        {
            entity.Name = entity.Name.ToUpper();
        }
    }
}
