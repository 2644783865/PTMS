using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Bs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public int? Azmth { get; set; }
    }
}
