namespace PTMS.BusinessLogic.Models
{
    public class BusStationRouteModel
    {
        public int Id { get; set; }
        public int? BusStationId { get; set; }
        public int? RouteId { get; set; }
        public int? Num { get; set; }
        public bool IsEndingStation { get; set; }
    }
}
