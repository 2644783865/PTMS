using PTMS.BusinessLogic.Models.EventLog;
using PTMS.Common;
using PTMS.Common.Enums;
using PTMS.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IEventLogService
    {
        Task<PageResult<EventLogModel>> FindByParams(
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
