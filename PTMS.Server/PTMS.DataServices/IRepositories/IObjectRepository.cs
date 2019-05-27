using System.Threading.Tasks;
using PTMS.Common;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

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
            int? page,
            int? pageSize);

        Task<Objects> GetByIdAsync(decimal id);

        Task<Objects> GetFullByIdAsync(decimal id);
    }
}
