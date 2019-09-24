using PTMS.BusinessLogic.Models.Dispatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IDispatchService
    {
        Task<List<TrolleybusTodayStatusModel>> GetTrolleybusTodayStatus();
    }
}
