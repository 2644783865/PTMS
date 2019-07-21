using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.BusinessLogic.Models;

namespace PTMS.BusinessLogic.IServices
{
    public interface IProjectService
    {
        Task<List<ProjectModel>> GetAllAsync(bool? active);

        Task<ProjectModel> GetByIdAsync(int id);
        Task<ProjectModel> GetByRouteIdAsync(int routeId);

        Task<ProjectModel> AddAsync(ProjectModel model);

        Task<ProjectModel> UpdateAsync(ProjectModel model);

        Task DeleteByIdAsync(int id);
    }
}
