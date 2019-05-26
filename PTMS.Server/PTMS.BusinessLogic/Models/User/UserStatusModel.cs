using PTMS.Common;
using PTMS.Domain.Enums;

namespace PTMS.BusinessLogic.Models.User
{
    public class UserStatusModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserStatusModel(UserStatusEnum userStatus)
        {
            Id = (int)userStatus;
            Name = EnumHelper.GetDescription(userStatus);
        }
    }
}
