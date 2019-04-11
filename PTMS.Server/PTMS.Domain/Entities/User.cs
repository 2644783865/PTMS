using Microsoft.AspNetCore.Identity;

namespace PTMS.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Description { get; set; }

        public int? TransporterId { get; set; }

        public virtual Transporter Transporter { get; }
    }
}
