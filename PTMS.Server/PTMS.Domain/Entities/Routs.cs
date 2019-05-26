﻿using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Routs
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool RouteActive { get; set; }
        public List<ProjectRoute> ProjectRoutes { get; set; }
    }
}
