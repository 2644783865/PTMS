using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IBusDataRepository
    {
        Task<List<TrolleybusTodayStatus>> GetTrolleybusTodayStatus();
        Task UpdateBusRoutes(Objects vehicle);
    }
}
