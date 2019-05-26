using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class RouteRepository : DataServiceAsync<Routs>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<List<Routs>> GetAllAsync(int? projectId, bool? active)
        {
            Expression<Func<Routs, bool>> filter = x =>
            (!active.HasValue || x.RouteActive == active.Value)
            && (!projectId.HasValue || x.ProjectRoutes.Any(p => p.ProjId == projectId.Value));

            return FindAsync(filter);
        }
    }
}
