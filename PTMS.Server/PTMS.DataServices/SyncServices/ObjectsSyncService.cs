using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System.Collections.Generic;

namespace PTMS.DataServices.SyncServices
{
    public class ObjectsSyncService : DataSyncService<Objects>
    {
        public ObjectsSyncService(
            ApplicationDbContext dbContext,
            IOptions<AppSettings> appSettings)
            :base(dbContext, appSettings)
        {
        }

        protected override List<string> GetPropertyNamesToSync()
        {
            var result = new List<string>
            {
                nameof(Objects.Id),
                nameof(Objects.ProjId),
                nameof(Objects.ObjId),
                nameof(Objects.VehicleType),
                nameof(Objects.Name),
                nameof(Objects.CarBrandId),
                nameof(Objects.ProviderId),
                nameof(Objects.LastRout),
                nameof(Objects.LastTime),
                nameof(Objects.ObjOutput),
                nameof(Objects.Phone)
            };

            return result;
        }
    }
}
