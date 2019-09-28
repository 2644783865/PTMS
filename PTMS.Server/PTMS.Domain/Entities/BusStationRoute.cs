namespace PTMS.Domain.Entities
{
    public class BusStationRoute
    {
        public int Id { get; set; }
        public int? BusStationId { get; set; }
        public int? RouteId { get; set; }
        public int? Num { get; set; }
        public bool IsEndingStation { get; set; }

        public BusStation BusStation { get; set; }
    }
}
