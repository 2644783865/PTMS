using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class RoleRepository : DataServiceAsync<AppRole>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<List<AppRole>> GetAllAsync()
        {
            return base.GetAllAsync();
        }
    }
}
