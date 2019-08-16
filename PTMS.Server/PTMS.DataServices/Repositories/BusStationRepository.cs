﻿using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class BusStationRepository : DataServiceAsync<BusStation>, IBusStationRepository
    {
        public BusStationRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task<List<BusStation>> GetAllAsync()
        {
            var list = await base.GetAllAsync();
            return list.OrderBy(x => x.Name).ToList();
        }
    }
}
