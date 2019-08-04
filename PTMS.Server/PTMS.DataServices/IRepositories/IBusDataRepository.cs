using PTMS.Domain.Entities;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IBusDataRepository
    {
        Task UpdateBusRoutes(Objects vehicle);
    }
}
