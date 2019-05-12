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
        private readonly string[] _includes = new[]
        {
            nameof(AppUser.UserRoles),
            nameof(AppUser.UserRoles) + "." + nameof(AppUserRole.Role)
        };

        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<List<AppUser>> GetAllWithRolesAsync()
        {
            return GetAllAsync(_includes);
        }

        public Task<AppUser> GetByIdWithRolesAsync(int id)
        {
            return GetByIdAsync(id, _includes);
        }
    }
}
