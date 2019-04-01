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
    public class TransporterService : BusinessServiceAsync<Transporter, TransporterModel>, ITransporterService
    {
        private readonly ITransporterRepository _transporterRepository;

        public TransporterService(
            ITransporterRepository transporterRepository,
            IMapper mapper)
            : base(mapper)
        {
            _transporterRepository = transporterRepository;
        }

        public async Task<List<TransporterModel>> GetAllAsync()
        {
            var result = await _transporterRepository.GetAllAsync();
            return MapToModel(result);
        }

        public async Task<TransporterModel> GetByIdAsync(int id)
        {
            var result = await _transporterRepository.GetByIdAsync(id);
            return MapToModel(result);
        }

        public async Task<TransporterModel> AddAsync(TransporterModel model)
        {
            var entity = MapFromModel(model);
            var result = await _transporterRepository.AddAsync(entity, true);
            return MapToModel(result);
        }

        public async Task<TransporterModel> UpdateAsync(TransporterModel model)
        {
            var entity = MapFromModel(model);
            var result = await _transporterRepository.UpdateAsync(entity, true);
            return MapToModel(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _transporterRepository.DeleteByIdAsync(id, true);
        }
    }
}
