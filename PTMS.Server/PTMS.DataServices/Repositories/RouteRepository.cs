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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class RouteRepository : DataServiceAsync<Route>, IRouteRepository
    {
        public RouteRepository(
            ApplicationDbContext context,
            RouteSyncService syncService,
            IDataChangeEventEmitter dataChangeEventEmitter)
            : base(context, syncService, dataChangeEventEmitter)
        {

        }

        public async Task<List<Route>> GetAllAsync(
            UserAvailableRoutes userRoutesModel,
            int? projectId,
            bool? active)
        {
            if (userRoutesModel == null)
            {
                userRoutesModel = new UserAvailableRoutes();
            }

            if (userRoutesModel.ProjectId.HasValue)
            {
                projectId = userRoutesModel.ProjectId;
            }

            Expression<Func<Route, bool>> filter = x =>
                (!active.HasValue || x.RouteActive == active.Value)
                && (!projectId.HasValue || x.ProjectRoutes.Any(p => p.ProjectId == projectId.Value))
                && (userRoutesModel.RouteIds == null || userRoutesModel.RouteIds.Contains(x.Id));

            var result = await FindAsync(filter);

            SortRoutes(result);
            return result;
        }

        public async Task<List<Route>> GetAllForPageAsync()
        {
            var includes = new[]
            {
                nameof(Route.ProjectRoutes)
            };

            var result = await base.GetAllAsync(includes);

            SortRoutes(result);
            return result;
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

        public async Task<List<Route>> GetActiveWithStationsAsync()
        {
            var includes = new[]
            {
                nameof(Route.BusStationRoutes),
                nameof(Route.BusStationRoutes) + "." + nameof(BusStationRoute.BusStation)
            };

            var result = await FindAsync(x => x.RouteActive, includes);

            SortRoutes(result);
            return result;
        }

        private void SortRoutes(List<Route> routes)
        {
            routes.Sort((first, second) =>
            {
                var compareResult = -first.RouteActive.CompareTo(second.RouteActive);

                if (compareResult == 0)
                {
                    compareResult = first.IsTrolleybus.CompareTo(second.IsTrolleybus);

                    if (compareResult == 0)
                    {
                        int firstNumber, secondNumber;
                        var firstParseResult = int.TryParse(Regex.Match(first.Name, @"\d+").Value, out firstNumber);
                        var secondParseResult = int.TryParse(Regex.Match(second.Name, @"\d+").Value, out secondNumber);

                        compareResult = -firstParseResult.CompareTo(secondParseResult);

                        if (compareResult == 0)
                        {
                            compareResult = firstNumber.CompareTo(secondNumber);

                            if (compareResult == 0)
                            {
                                var firstLetters = Regex.Replace(first.Name, @"[\d-]", string.Empty);
                                var secondLetters = Regex.Replace(second.Name, @"[\d-]", string.Empty);
                                compareResult = firstLetters.CompareTo(secondLetters);
                            }
                        }
                    }
                }

                return compareResult;
            });
        }
    }
}
