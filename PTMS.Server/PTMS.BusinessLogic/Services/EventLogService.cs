using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.EventLog;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class EventLogService : BusinessServiceAsync<EventLog>, IEventLogService
    {
        private readonly IEventLogRepository _eventLogRepository;

        public EventLogService(
            IEventLogRepository eventLogRepository,
            IMapper mapper)
            : base(mapper)
        {
            _eventLogRepository = eventLogRepository;
        }

        public async Task<PageResult<EventLogModel>> FindByParams(
            string entityType,
            int? entityId,
            EventEnum? eventEnum,
            int? userId,
            DateTime? startDate,
            DateTime? endDate,
            string fieldName,
            string fieldValue,
            bool onlyProject,
            OrderByEnum orderBy,
            int? page,
            int? pageSize)
        {
            var dataResult = await _eventLogRepository.FindByParamsAsync(
                entityType,
                entityId,
                eventEnum,
                userId,
                startDate,
                endDate,
                fieldName,
                fieldValue,
                onlyProject,
                orderBy,
                page,
                pageSize);

            var result = MapToModel<EventLogModel>(dataResult);
            return result;
        }
    }
}
