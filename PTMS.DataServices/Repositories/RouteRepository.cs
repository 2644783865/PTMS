using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class RouteRepository : DataServiceAsync<Route>, IRouteRepository
    {
        public RouteRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<List<Route>> GetAllAsync()
        {
            return base.GetAllAsync();
        }
    }
}
