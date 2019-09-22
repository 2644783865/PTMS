using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.EventLog;
using PTMS.BusinessLogic.Models.Shared;
using PTMS.Common;
using PTMS.Domain.Enums;
using System;
using System.Collections.Generic;
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

        [HttpGet("/eventEntityTypes")]
        public ActionResult<List<NamedEntityModel>> GetEntityTypes()
        {
            var result = new List<NamedEntityModel>()
            {
                new NamedEntityModel
                {
                    Id = 1,
                    Name = "Objects"
                }
            };

            return result;
        }

        [HttpGet("/eventOperations")]
        public ActionResult<List<NamedEntityModel>> GetOperations()
        {
            var result = new List<NamedEntityModel>();

            foreach (EventEnum eventEnum in Enum.GetValues(typeof(EventEnum)))
            {
                result.Add(new NamedEntityModel
                {
                    Id = (int)eventEnum,
                    Name = EnumHelper.GetDescription(eventEnum)
                });
            }

            return result;
        }
    }
}
