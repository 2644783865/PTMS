using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IProjectRepository : IDataServiceAsync<Project>
    {
        Task<List<Project>> GetAllAsync(bool? active);

        Task<Project> GetProjectByRouteIdAsync(int routeId);
    }
}
