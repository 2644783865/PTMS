using System;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class BsRoute
    {
        public int Id { get; set; }
        public int? BsId { get; set; }
        public int? RouteId { get; set; }
        public int? Num { get; set; }
    }
}
