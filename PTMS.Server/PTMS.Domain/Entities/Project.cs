using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Director { get; set; }

        public List<ProjectRoute> ProjectRoutes { get; set; }
    }
}
