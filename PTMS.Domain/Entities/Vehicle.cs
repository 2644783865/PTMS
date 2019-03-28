namespace PTMS.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; }
        
        public int RouteId { get; set; }
        
        public int TransporterId { get; set; }
        
        public int VehicleTypeId { get; set; }

        public Transporter Transporter { get; set; }
        public Route Route { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
