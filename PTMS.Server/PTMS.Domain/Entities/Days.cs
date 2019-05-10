using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Days
    {
        public int DId { get; set; }
        public string DayName { get; set; }
        public string DayShortName { get; set; }
        public short DayType { get; set; }
    }
}
