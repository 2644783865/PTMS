using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models.Account
{
    public class RegisterModel
    {
        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
