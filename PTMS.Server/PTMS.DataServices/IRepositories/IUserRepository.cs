using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IUserRepository : IDataServiceAsync<User>
    {
        Task<List<User>> GetAllAsync();
    }
}
