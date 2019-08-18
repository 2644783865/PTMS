using PTMS.BusinessLogic.Models;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IBusStationRouteService
    {
        Task<BusStationRouteModel> AddAsync(BusStationRouteModel model);
        Task<BusStationRouteModel> UpdateAsync(int id, BusStationRouteModel model);
    }
}
