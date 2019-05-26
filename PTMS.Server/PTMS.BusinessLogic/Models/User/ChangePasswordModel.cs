using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models.User
{
    public class ChangePasswordModel
    {
        [Required]
        public string Password { get; set; }
    }
}
