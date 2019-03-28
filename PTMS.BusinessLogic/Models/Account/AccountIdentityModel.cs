using System.Collections.Generic;

namespace PTMS.BusinessLogic.Models.Account
{
    public class AccountIdentityModel
    {
        public IList<string> Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
