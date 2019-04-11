using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class UserRepository : DataServiceAsync<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<List<User>> GetAllAsync()
        {
            return base.GetAllAsync();
        }
    }
}
