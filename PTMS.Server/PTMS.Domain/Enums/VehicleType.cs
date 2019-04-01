using System.ComponentModel;

namespace PTMS.Domain.Enums
{
    public enum VehicleType
    {
        [Description("Автобус")]
        Bus = 1,

        [Description("Троллейбус")]
        Trolleybus,

        [Description("Маршрутное такси")]
        ShuttleBus
    }
}
