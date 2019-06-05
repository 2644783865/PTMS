using System.Collections.Generic;

namespace PTMS.BusinessLogic.Models.Account
{
    public class AccountIdentityModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public int? ProjectId { get; set; }
        public List<int> RouteIds { get; set; }
    }
}
