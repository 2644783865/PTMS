using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IProjectRouteRepository : IDataServiceAsync<ProjectRoute>
    {
        Task<List<ProjectRoute>> GetAllActiveAsync();
    }
}
