using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface IProviderService
    {
        Task<List<ProviderModel>> GetAllAsync();
    }
}
