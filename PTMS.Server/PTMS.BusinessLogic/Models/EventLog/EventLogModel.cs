using PTMS.BusinessLogic.Models.User;
using PTMS.Common.Enums;
using PTMS.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PTMS.BusinessLogic.Models.EventLog
{
    public class EventLogModel
    {
        public int Id { get; set; }
        public UserLightModel User { get; set; }
        public DateTime TimeStamp { get; set; }
        public EventEnum Event { get; set; }
        public string EventName => EnumHelper.GetDescription(Event);
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Message { get; set; }
        public List<EventLogFieldModel> EventLogFields { get; set; }
    }
}
