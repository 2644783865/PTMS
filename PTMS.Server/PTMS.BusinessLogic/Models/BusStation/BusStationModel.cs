﻿using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models
{
    public class BusStationModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double? Latitude { get; set; }

        [Required]
        public double? Longitude { get; set; }

        [Required]
        public int? Azimuth { get; set; }
    }
}
