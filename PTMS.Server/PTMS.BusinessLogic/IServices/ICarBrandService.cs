using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface ICarBrandService
    {
        Task<List<CarBrandModel>> GetAllAsync();

        Task<CarBrandModel> AddAsync(CarBrandModel model);

        Task<CarBrandModel> UpdateAsync(CarBrandModel model);

        Task DeleteByIdAsync(int id);
    }
}
