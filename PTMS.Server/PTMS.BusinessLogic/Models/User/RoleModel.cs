using System.ComponentModel.DataAnnotations;

namespace PTMS.BusinessLogic.Models.User
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
