using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class ProviderService : BusinessServiceAsync<Provider>, IProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(
            IProviderRepository providerRepository,
            IMapper mapper)
            : base(mapper)
        {
            _providerRepository = providerRepository;
        }

        public async Task<List<ProviderModel>> GetAllAsync()
        {
            var result = await _providerRepository.GetAllAsync();
            return MapToModel<ProviderModel>(result);
        }
    }
}
