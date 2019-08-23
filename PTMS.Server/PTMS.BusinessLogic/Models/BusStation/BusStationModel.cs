using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models
{
    public class BusStationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double? Lat { get; set; }

        [Required]
        public double? Lon { get; set; }

        [Required]
        public int? Azmth { get; set; }
    }
}
