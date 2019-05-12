using System.ComponentModel;

namespace PTMS.Domain.Enums
{
    public enum UserStatusEnum
    {
        [Description("Активный")]
        Active = 1,

        [Description("Ожидает подтверждения")]
        WaitForConfirmation = 2,

        [Description("Не активный")]
        Disabled = 3,

        [Description("Заблокирован")]
        Locked = 4
    }
}
