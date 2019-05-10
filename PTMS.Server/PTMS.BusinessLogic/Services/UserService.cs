using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class UserService : BusinessServiceAsync<AppUser, UserModel>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
            : base(mapper)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserModel>> GetAllWithRolesAsync()
        {
            var users = await _userRepository.GetAllWithRolesAsync();
            var result = new List<UserModel>();

            foreach (var user in users)
            {
                var userModel = MapToModel(user);
                userModel.Role = user.UserRoles.First().Role.DisplayName;
            }

            return result;
        }
    }
}
