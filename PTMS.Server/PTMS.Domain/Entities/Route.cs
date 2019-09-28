using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool RouteActive { get; set; }

        public bool IsTrolleybus
        {
            get { return Name.StartsWith("Т", System.StringComparison.InvariantCultureIgnoreCase); }
        }

        public List<ProjectRoute> ProjectRoutes { get; set; }
        public List<BusStationRoute> BusStationRoutes { get; set; }
    }
}
