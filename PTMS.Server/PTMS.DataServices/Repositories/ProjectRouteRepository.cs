using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.Repositories
{
    public class ProjectRouteRepository : DataServiceAsync<ProjectRoute>, IProjectRouteRepository
    {
        public ProjectRouteRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<List<ProjectRoute>> GetAllActiveAsync()
        {
            return FindAsync(x => x.Route.RouteActive);
        }
    }
}
