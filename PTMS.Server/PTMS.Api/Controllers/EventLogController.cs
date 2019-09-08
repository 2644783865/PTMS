using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.EventLog;
using PTMS.Common;
using PTMS.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class EventLogController : ApiControllerBase
    {
        private readonly IEventLogService _eventLogService;

        public EventLogController(IEventLogService eventLogService)
        {
            _eventLogService = eventLogService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/eventLogs")]
        public async Task<ActionResult<PageResult<EventLogModel>>> GetAll(
            string entityType,
            int? entityId,
            EventEnum? eventEnum,
            int? userId,
            DateTime? startDate,
            DateTime? endDate,
            string fieldName,
            string fieldValue,
            int? page,
            int? pageSize,
            OrderByEnum orderBy = OrderByEnum.Desc)
        {
            var result = await _eventLogService.FindByParams(
                entityType,
                entityId,
                eventEnum,
                userId,
                startDate,
                endDate,
                fieldName,
                fieldValue,
                orderBy,
                page,
                pageSize);

            return result;
        }
    }
}
