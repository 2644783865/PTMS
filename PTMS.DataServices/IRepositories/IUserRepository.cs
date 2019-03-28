using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IUserRepository : IDataServiceAsync<User>
    {
    }
}
