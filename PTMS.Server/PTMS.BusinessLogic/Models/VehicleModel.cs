using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }

        [Required]
        public string PlateNumber { get; set; }

        [Required]
        public int RouteId { get; set; }

        [Required]
        public int TransporterId { get; set; }

        [Required]
        public int VehicleTypeId { get; set; }

        public TransporterModel Transporter { get; set; }
        public RouteModel Route { get; set; }
        public VehicleTypeModel VehicleType { get; set; }
    }
}
