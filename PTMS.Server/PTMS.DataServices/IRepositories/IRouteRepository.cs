using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IRouteRepository : IDataServiceAsync<Routs>
    {
        Task<List<Routs>> GetAllAsync(
            UserAvailableRoutes userRoutesModel,
            int? projectId,
            bool? active);
    }
}
