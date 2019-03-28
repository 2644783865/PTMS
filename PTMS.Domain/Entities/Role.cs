using Microsoft.AspNetCore.Identity;

namespace PTMS.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public string DisplayName { get; set; }
    }
}
