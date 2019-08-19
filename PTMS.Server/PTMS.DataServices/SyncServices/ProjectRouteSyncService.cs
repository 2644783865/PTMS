using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.SyncServices
{
    public class ProjectRouteSyncService : DataSyncService<ProjectRoute>
    {
        public ProjectRouteSyncService(
            ApplicationDbContext dbContext,
            IOptions<AppSettings> appSettings)
            :base(dbContext, appSettings)
        {
        }
    }
}
