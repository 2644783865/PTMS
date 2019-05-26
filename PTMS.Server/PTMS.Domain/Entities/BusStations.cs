using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class BusStations
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public double? Lon { get; set; }
        public double? Lat { get; set; }
        public int Rout { get; set; }
        public short Control { get; set; }
        public int Id { get; set; }
    }
}
