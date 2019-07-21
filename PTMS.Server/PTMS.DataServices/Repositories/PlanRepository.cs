using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class PlanRepository : DataServiceAsync<Plans>, IPlanRepository
    {
        public PlanRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<List<Plans>> GetByDateAsync(DateTime date)
        {
            return FindAsync(x => x.Date == date.Date);
        }
    }
}
