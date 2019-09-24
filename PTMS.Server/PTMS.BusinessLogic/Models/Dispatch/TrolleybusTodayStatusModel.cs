using System;

namespace PTMS.BusinessLogic.Models.Dispatch
{
    public class TrolleybusTodayStatusModel
    {
        public ObjectModel Trolleybus { get; set; }
        public string Place { get; set; }
        public RouteModel NewRoute { get; set; }
        public DateTime? CoordinationTime { get; set; }
    }
}
