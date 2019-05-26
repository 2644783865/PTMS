using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class ReportsControl
    {
        public int RcId { get; set; }
        public DateTime? RepCreateDate { get; set; }
        public int RepCheck { get; set; }
        public DateTime? RepCheckDate { get; set; }
        public int? RepCreateUser { get; set; }
        public int? RepCheckUser { get; set; }
        public int RepMail { get; set; }
        public string RepMailTo { get; set; }
        public DateTime? RepMailDate { get; set; }
        public int? RepMailUser { get; set; }
        public byte[] RepDataFr3 { get; set; }
        public string Comments { get; set; }
    }
}
