using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IEventLogRepository : IDataServiceAsync<EventLog>
    {
        Task<PageResult<EventLog>> FindByParamsAsync(
            string entityType,
            int? entityId,
            string entityName,
            EventEnum? eventEnum,
            int? userId,
            DateTime? startDate,
            DateTime? endDate,
            string fieldName,
            string fieldValue,
            bool onlyProject,
            OrderByEnum orderBy,
            int? page,
            int? pageSize);
    }
}
