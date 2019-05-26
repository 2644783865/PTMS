using Microsoft.AspNetCore.Identity;

namespace PTMS.Domain.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; private set; }
        public AppRole Role { get; private set; }
    }
}
