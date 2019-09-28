using PTMS.BusinessLogic.Models.Object;
using System;

namespace PTMS.BusinessLogic.Models.Dispatch
{
    public class TrolleybusTodayStatusModel
    {
        public int Id => Trolleybus.Id;
        public ObjectModel Trolleybus { get; set; }
        public string Place { get; set; }
        public RouteModel NewRoute { get; set; }
        public DateTime? CoordinationTime { get; set; }
        public bool IsNotDefined { get; set; }
    }
}
