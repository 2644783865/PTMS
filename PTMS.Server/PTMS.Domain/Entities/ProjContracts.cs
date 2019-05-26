using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class ProjContracts
    {
        public int Ids { get; set; }
        public string Number { get; set; }
        public DateTime? DateStart { get; set; }
        public int? ProjId { get; set; }
        public string Atribut { get; set; }
    }
}
