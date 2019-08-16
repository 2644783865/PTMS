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
    public class ProjectService : BusinessServiceAsync<Project>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IMapper mapper)
            : base(mapper)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectModel>> GetAllAsync(bool? active)
        {
            var result = await _projectRepository.GetAllAsync(active);
            return MapToModel<ProjectModel>(result);
        }

        public async Task<ProjectModel> GetByRouteIdAsync(int routeId)
        {
            var result = await _projectRepository.GetProjectByRouteIdAsync(routeId);
            return MapToModel<ProjectModel>(result);
        }

        public async Task<ProjectModel> GetByIdAsync(int id)
        {
            var result = await _projectRepository.GetByIdAsync(id);
            return MapToModel<ProjectModel>(result);
        }

        public async Task<ProjectModel> AddAsync(ProjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _projectRepository.AddAsync(entity);
            return MapToModel<ProjectModel>(result);
        }

        public async Task<ProjectModel> UpdateAsync(ProjectModel model)
        {
            var entity = MapFromModel(model);
            var result = await _projectRepository.UpdateAsync(entity);
            return MapToModel<ProjectModel>(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _projectRepository.DeleteByIdAsync(id);
        }
    }
}
