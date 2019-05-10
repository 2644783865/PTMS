using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace PTMS.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Description { get; set; }

        public int? ProjectId { get; set; }

        public bool Enabled { get; set; }

        public virtual Project Project { get; set; }

        public virtual List<AppUserRole> UserRoles { get; set; }
    }
}
