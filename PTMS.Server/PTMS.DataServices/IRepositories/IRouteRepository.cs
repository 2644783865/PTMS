using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IRouteRepository : IDataServiceAsync<Routs>
    {
        Task<List<Routs>> GetAllAsync(int? projectId, bool? active);
    }
}
