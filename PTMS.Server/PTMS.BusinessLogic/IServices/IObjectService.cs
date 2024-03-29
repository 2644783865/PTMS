﻿using PTMS.BusinessLogic.Models.Object;
using PTMS.BusinessLogic.Models.Shared;
using PTMS.Common;
using PTMS.Common.Enums;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IObjectService
    {
        Task<PageResult<ObjectModel>> FindByParamsAsync(
            ClaimsPrincipal user,
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string blockNumber,
            int? blockTypeId,
            string sortBy,
            OrderByEnum orderBy,
            int? page,
            int? pageSize);

        Task<FileModel> GetVehiclesFileAsync(
            ClaimsPrincipal user,
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            bool? active,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string blockNumber,
            int? blockTypeId,
            string sortBy,
            OrderByEnum orderBy,
            FileFormatEnum fileFormat);

        Task<ObjectModel> GetByIdAsync(int ids);

        Task<ObjectModel> AddAsync(
            ObjectAddEditRequest model,
            ClaimsPrincipal principal);

        Task<ObjectModel> UpdateAsync(
            int id, 
            ObjectAddEditRequest model,
            ClaimsPrincipal principal);

        Task<ObjectModel> ChangeRouteAsync(
            int ids,
            int newRouteId,
            ClaimsPrincipal principal,
            bool updateBusRoutes);

        Task<ObjectModel> EnableAsync(
            int ids,
            int newRouteId,
            ClaimsPrincipal principal);

        Task<ObjectModel> DisableAsync(
            int ids,
            ClaimsPrincipal principal);

        Task DeleteByIdAsync(
            int id,
            ClaimsPrincipal principal);
        
        Task<List<ObjectModel>> FindForReportingAsync(int minutes);
    }
}
