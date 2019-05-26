using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class IbeTodo
    {
        public int ItemId { get; set; }
        public string ObjectName { get; set; }
        public int? ObjectType { get; set; }
        public short ItemPriority { get; set; }
        public short ItemState { get; set; }
        public string ItemCaption { get; set; }
        public string ItemDescription { get; set; }
        public string ResponsiblePerson { get; set; }
        public DateTime ItemTimestamp { get; set; }
        public DateTime? ItemDeadline { get; set; }
        public string ItemCategory { get; set; }
        public string ItemOwner { get; set; }
    }
}
