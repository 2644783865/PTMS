namespace PTMS.BusinessLogic.Models
{
    public class BusStationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public int? Azmth { get; set; }
    }
}
