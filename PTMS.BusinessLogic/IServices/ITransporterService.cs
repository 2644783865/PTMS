using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface ITransporterService
    {
        Task<List<TransporterModel>> GetAllAsync();

        Task<TransporterModel> GetByIdAsync(int id);

        Task<TransporterModel> AddAsync(TransporterModel model);

        Task<TransporterModel> UpdateAsync(TransporterModel model);

        Task DeleteByIdAsync(int id);
    }
}
