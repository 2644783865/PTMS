using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PTMS.DataServices.Infrastructure;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Persistance;

namespace PTMS.DataServices.Repositories
{
    public class CarBrandRepository : DataServiceAsync<CarBrand>, ICarBrandRepository
    {
        public CarBrandRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public async Task<List<CarBrand>> GetAllAsync()
        {
            var list = await base.GetAllAsync();
            return list.OrderBy(x => x.Name).ToList();
        }
    }
}
