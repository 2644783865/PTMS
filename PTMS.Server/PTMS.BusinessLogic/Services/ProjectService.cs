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
    public class ProjectService : BusinessServiceAsync<Project, ProjectModel>, IProjectService
    {
        private readonly IProjectRepository _transporterRepository;

        public ProjectService(
            IProjectRepository transporterRepository,
            IMapper mapper)
            : base(mapper)
        {
            _transporterRepository = transporterRepository;
        }

        public async Task<List<ProjectModel>> GetAllAsync()
        {
            var result = await _transporterRepository.GetAllAsync();
            return MapToModel(result);
        }

        public async Task<ProjectModel> GetByIdAsync(int id)
        {
            var result = await _transporterRepository.GetByIdAsync(id);
            return MapToModel(result);
        }

        public async Task<ProjectModel> AddAsync(ProjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _transporterRepository.AddAsync(entity, true);
            return MapToModel(result);
        }

        public async Task<ProjectModel> UpdateAsync(ProjectModel model)
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
