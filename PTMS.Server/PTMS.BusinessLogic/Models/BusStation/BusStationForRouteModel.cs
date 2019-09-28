namespace PTMS.BusinessLogic.Models
{
    public class BusStationForRouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Azimuth { get; set; }
        public int? Num { get; set; }
        public bool IsEndingStation { get; set; }
    }
}
