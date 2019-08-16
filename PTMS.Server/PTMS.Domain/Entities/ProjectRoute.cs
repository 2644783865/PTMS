using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class ProjectRoute
    {
        public int Ids { get; set; }
        public int ProjId { get; set; }
        public int RoutId { get; set; }

        public Project Project { get; set; }
        public Route Route { get; set; }
    }
}
