using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.SyncServices
{
    public class BusStationRouteSyncService : DataSyncService<BusStationRoute>
    {
        public BusStationRouteSyncService(
            ApplicationDbContext dbContext,
            IOptions<AppSettings> appSettings)
            :base(dbContext, appSettings)
        {
        }
    }
}
