using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class GranitRepository : DataServiceAsync<Granit>, IGranitRepository
    {
        public GranitRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}
