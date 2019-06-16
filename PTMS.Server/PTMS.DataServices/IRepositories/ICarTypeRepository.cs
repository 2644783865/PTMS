using System.Collections.Generic;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.Domain.Entities;

namespace PTMS.DataServices.IRepositories
{
    public interface ICarTypeRepository : IDataServiceAsync<CarType>
    {
        Task<List<CarType>> GetAllAsync();
    }
}
