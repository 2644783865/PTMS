using System.Collections.Generic;

namespace PTMS.BusinessLogic.Models
{
    public class RouteFullModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool RouteActive { get; set; }
        public ProjectModel Project { get; set; }
        public List<BusStationRouteModel> BusStationRoutes { get; set; }
    }
}
