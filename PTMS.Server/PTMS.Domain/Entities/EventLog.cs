using PTMS.Domain.Enums;
using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class EventLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public EventEnum Event { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string Message { get; set; }

        public AppUser User { get; set; }
        public List<EventLogField> EventLogFields { get; set; }
    }
}
