using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models.User
{
    public class ConfirmUserModel
    {
        [Required]
        public int RoleId { get; set; }

        public int? ProjectId { get; set; }
    }
}
