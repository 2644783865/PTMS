using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class RouteRepository : DataServiceAsync<Routs>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task<List<Routs>> GetAllAsync(
            UserAvailableRoutes userRoutesModel,
            int? projectId,
            bool? active)
        {
            if (userRoutesModel.ProjectId.HasValue)
            {
                projectId = userRoutesModel.ProjectId;
            }

            Expression<Func<Routs, bool>> filter = x =>
                (!active.HasValue || x.RouteActive == active.Value)
                && (!projectId.HasValue || x.ProjectRoutes.Any(p => p.ProjId == projectId.Value))
                && (userRoutesModel.RouteIds == null || userRoutesModel.RouteIds.Contains(x.Id));

            var result = await FindAsync(filter);

            return result.OrderBy(x => x.Name).ToList();
        }
    }
}
