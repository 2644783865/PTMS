﻿using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class CarBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CarTypeId { get; set; }
        public string L { get; set; }
        public string W { get; set; }
        public string H { get; set; }

        public virtual CarType CarType { get; set; }

        public virtual List<Objects> Objects { get; set; }
    }
}
