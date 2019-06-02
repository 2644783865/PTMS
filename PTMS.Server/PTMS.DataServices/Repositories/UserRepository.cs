using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class UserRepository : DataServiceAsync<AppUser>, IUserRepository
    {
        private readonly string[] _includesFull = new[]
        {
            nameof(AppUser.UserRoles),
            nameof(AppUser.UserRoles) + "." + nameof(AppUserRole.Role),
            nameof(AppUser.Project)
        };

        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<List<AppUser>> GetAllFullAsync()
        {
            return GetAllAsync(_includesFull);
        }

        public Task<AppUser> GetByIdFullAsync(int id)
        {
            return GetByIdAsync(id, _includesFull);
        }
    }
}
