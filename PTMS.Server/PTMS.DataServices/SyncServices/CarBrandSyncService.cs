using System.Collections.Generic;
using Microsoft.Extensions.Options;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.SyncServices
{
    public class CarBrandSyncService : DataSyncService<CarBrand>
    {
        public CarBrandSyncService(
            ApplicationDbContext  dbContext,
            IOptions<AppSettings> appSettings)
            :base(dbContext, appSettings)
        {
        }

        protected override List<string> GetPropertyNamesToSync()
        {
            var result = new List<string>
            {
                nameof(CarBrand.Id),
                nameof(CarBrand.Name),
                nameof(CarBrand.CarTypeId)
            };

            return result;
        }
    }
}
