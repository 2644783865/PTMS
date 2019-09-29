using PTMS.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace PTMS.BusinessLogic.Models
{
    public class RouteWithStationsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RouteDirectionTypeEnum Type => BackDirectionStations != null && BackDirectionStations.Any()
            ? RouteDirectionTypeEnum.Plain
            : RouteDirectionTypeEnum.Circle;

        public List<BusStationForRouteModel> ForwardDirectionStations { get; set; }

        public List<BusStationForRouteModel> BackDirectionStations { get; set; }
    }
}
