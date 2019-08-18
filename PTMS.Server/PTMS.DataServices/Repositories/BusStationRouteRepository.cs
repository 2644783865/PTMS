using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.SyncServices;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class BusStationRouteRepository : DataServiceAsync<BusStationRoute>, IBusStationRouteRepository
    {
        public BusStationRouteRepository(
            ApplicationDbContext context,
            BusStationRouteSyncService syncService)
            : base(context, syncService)
        {

        }
    }
}
