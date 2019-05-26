using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class WebMapUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string FactAddrLon { get; set; }
        public string FactAddrLat { get; set; }
    }
}
