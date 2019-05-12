using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IRoleRepository : IDataServiceAsync<AppRole>
    {
        Task<List<AppRole>> GetAllAsync();
    }
}
