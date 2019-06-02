using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IUserRepository : IDataServiceAsync<AppUser>
    {
        Task<List<AppUser>> GetAllFullAsync();
        Task<AppUser> GetByIdFullAsync(int id);
    }
}
