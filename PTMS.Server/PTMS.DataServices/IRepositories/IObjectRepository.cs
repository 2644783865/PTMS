using System.Threading.Tasks;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface IObjectRepository : IDataServiceAsync<Objects>
    {
        Task<PageResult<Objects>> FindByParamsForPageAsync(
            string plateNumber,
            string routeName,
            int? vehicleTypeId,
            int? projectId,
            int? page,
            int? pageSize);

        Task<Objects> GetByIdForPageAsync(int id);
    }
}
