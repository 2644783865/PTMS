using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class EventLogFieldRepository : DataServiceAsync<EventLogField>, IEventLogFieldRepository
    {
        public EventLogFieldRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
