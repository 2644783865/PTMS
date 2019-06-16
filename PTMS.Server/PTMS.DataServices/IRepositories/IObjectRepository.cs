using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.Models;
using PTMS.Domain.Entities;
using System.Threading.Tasks;

namespace PTMS.DataServices.IRepositories
{
    public interface IObjectRepository : IDataServiceAsync<Objects>
    {
        Task<PageResult<Objects>> FindByParamsAsync(
            string plateNumber,
            string routeName,
            int? carTypeId,
            int? projectId,
            ModelFormatsEnum format,
            bool? active,
            UserAvailableRoutes userRoutesModel,
            int? carBrandId,
            int? providerId,
            int? yearRelease,
            int? page,
            int? pageSize);

        Task<Objects> GetByIdAsync(decimal id);

        Task<Objects> GetFullByIdAsync(decimal id);
    }
}
