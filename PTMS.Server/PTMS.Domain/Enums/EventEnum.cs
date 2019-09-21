using System.ComponentModel;

namespace PTMS.Domain.Enums
{
    public enum EventEnum
    {
        [Description("Добавление")]
        Create = 1,
        [Description("Редактирование")]
        Update = 2,
        [Description("Удаление")]
        Delete = 3,
        [Description("Смена маршрута ТС")]
        ChangeObjectRoute = 4,
        [Description("Ввод ТС в эксплуатацию")]
        EnableObject = 5,
        [Description("Вывод ТС из эксплуатации")]
        DisableObject = 6
    }
}
