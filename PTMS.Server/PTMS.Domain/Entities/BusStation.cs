namespace PTMS.Domain.Entities
{
    public class BusStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Azimuth { get; set; }
    }
}
