using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IRouteRepository : IDataServiceAsync<Route>
    {
        Task<List<Route>> GetAllAsync(
            UserAvailableRoutes userRoutesModel,
            int? projectId,
            bool? active);

        Task<Route> GetForEditByIdAsync(int id);

        Task<List<Route>> GetAllForPageAsync();

        Task<List<Route>> GetActiveWithStationsAsync();
    }
}
