﻿using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Providers
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Objects> Objects { get; set; }
    }
}
