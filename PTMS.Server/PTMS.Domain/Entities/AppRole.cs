using Microsoft.AspNetCore.Identity;

namespace PTMS.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public string DisplayName { get; set; }
    }
}
