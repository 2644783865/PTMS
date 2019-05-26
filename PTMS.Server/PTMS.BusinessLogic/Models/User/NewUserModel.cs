using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models.User
{
    public class NewUserModel
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

        [Required]
        public string Description { get; set; }

        [Required]
        public int RoleId { get; set; }

        public int? ProjectId { get; set; }
    }
}
