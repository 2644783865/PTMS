using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class TransporterRepository : DataServiceAsync<Transporter>, ITransporterRepository
    {
        public TransporterRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public Task<List<Transporter>> GetAllAsync()
        {
            return base.GetAllAsync();
        }
    }
}
