using System;

namespace PTMS.DataServices.Models
{
    public class FactOfObjectRoute
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateTimeOfOutput { get; set; }
        public int CountObjects { get; set; }
    }
}
