using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class Transporter
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Vehicle> Vehicles { get; set; }
    }
}
