using PTMS.BusinessLogic.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllFullAsync();
        Task<UserModel> GetByIdFullAsync(int id);
        Task<UserModel> CreateUserAsync(NewUserModel model);
        Task<UserModel> ConfirmUserAsync(int userId, ConfirmUserModel model);
        Task ChangePasswordAsync(int userId, string newPassword);
        Task<UserModel> ToggleUserStatusAsync(int id);
        Task<List<RoleModel>> GetAllRoles();
        Task<List<UserStatusModel>> GetAllStatuses();
    }
}
