using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class ProjectRepository : DataServiceAsync<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task<List<Project>> GetAllAsync()
        {
            var list = await base.GetAllAsync();
            return list.OrderBy(x => x.Name).ToList();
        }

        public Task<Project> GetProjectByRouteIdAsync(int routeId)
        {
            return GetAsync(x => x.ProjectRoutes.Any(y => y.RoutId == routeId));
        }
    }
}
