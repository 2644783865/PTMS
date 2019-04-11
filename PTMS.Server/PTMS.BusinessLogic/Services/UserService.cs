using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
    public class UserService : BusinessServiceAsync<User, UserModel>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public UserService(
            IUserRepository userRepository,
            UserManager<User> userManager,
            IMapper mapper)
            : base(mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<List<UserModel>> GetAllWithRolesAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var result = MapToModel(users);

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userModel = result.First(x => x.Id == user.Id);
                userModel.Role = roles.First();
            }

            return result;
        }
    }
}
