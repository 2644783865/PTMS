using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.DataServices.Models;
using PTMS.DataServices.SyncServices;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class RouteRepository : DataServiceAsync<Route>, IRouteRepository
    {
        public RouteRepository(
            ApplicationDbContext context,
            RouteSyncService syncService)
            : base(context, syncService)
        {

        }

        public async Task<List<Route>> GetAllAsync(
            UserAvailableRoutes userRoutesModel,
            int? projectId,
            bool? active)
        {
            if (userRoutesModel.ProjectId.HasValue)
            {
                projectId = userRoutesModel.ProjectId;
            }

            Expression<Func<Route, bool>> filter = x =>
                (!active.HasValue || x.RouteActive == active.Value)
                && (!projectId.HasValue || x.ProjectRoutes.Any(p => p.ProjectId == projectId.Value))
                && (userRoutesModel.RouteIds == null || userRoutesModel.RouteIds.Contains(x.Id));

            var result = await FindAsync(filter);

            return result.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<Route>> GetAllForPageAsync()
        {
            var includes = new[]
            {
                nameof(Route.ProjectRoutes)
            };

            var result = await base.GetAllAsync(includes);
            return result.OrderBy(x => x.Name).ToList();
        }

        public Task<Route> GetForEditByIdAsync(int id)
        {
            var includes = new[]
            {
                nameof(Route.BusStationRoutes),
                nameof(Route.ProjectRoutes)
            };

            return GetByIdAsync(id, includes);
        }
    }
}
