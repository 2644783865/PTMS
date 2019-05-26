using System.Collections.Generic;

namespace PTMS.BusinessLogic.Models.Account
{
    public class AccountIdentityModel
    {
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
