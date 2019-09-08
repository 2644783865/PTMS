using System.ComponentModel;

namespace PTMS.Domain.Enums
{
    public enum EventEnum
    {
        [Description("Добавление")]
        Create = 1,
        [Description("Обновление")]
        Update,
        [Description("Удаление")]
        Delete,
        [Description("Смена маршрута ТС")]
        ChangeObjectRoute,
        [Description("Ввод ТС в эксплуатацию")]
        EnableObject,
        [Description("Вывод ТС из эксплуатации")]
        DisableObject
    }
}
