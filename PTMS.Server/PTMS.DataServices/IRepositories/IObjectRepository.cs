using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IObjectRepository : IDataServiceExAsync<Objects, decimal>
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
        
        Task<Objects> GetFullByIdAsync(decimal id);

        Task<List<Objects>> FindForReporting(DateTime onlineStartDate, DateTime onlineEndDate);
    }
}
