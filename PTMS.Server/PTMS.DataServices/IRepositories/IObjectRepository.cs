using PTMS.Common;
using PTMS.Common.Enums;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IObjectRepository : IDataServiceAsync<Objects>
    {
        Task<PageResult<Objects>> FindByParamsAsync(
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
            int? pageSize);

        Task<bool> AnyByPlateNumberAsync(string name, int? currentEntityId);

        Task<bool> AnyByPhoneAsync(long phone, int? currentEntityId);

        Task<bool> AnyByObjIdProjIdAsync(long objId, long projId, int? currentEntityId);

        Task<Objects> GetFullByIdAsync(int id);

        Task<Objects> GetPureByIdAsync(int id);

        Task<Objects> GetByIdWithBlockAsync(int id);
        
        Task<List<Objects>> FindForReportingAsync(DateTime onlineStartDate, DateTime onlineEndDate);

        Task<List<Objects>> GetAllTrolleybuses();

        Task<short> GetNextObjectIdAsync();

        Task<List<Objects>> FindAllForFileAsync(
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            bool? active,
            UserAvailableRoutes userRoutesModel,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            string blockNumber,
            int? blockTypeId,
            string sortBy,
            OrderByEnum orderBy);
    }
}
