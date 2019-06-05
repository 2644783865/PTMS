using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PTMS.BusinessLogic.Config;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.User;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class UserService : BusinessServiceAsync<AppUser, UserModel>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly AppUserManager _userManager;
        private readonly AppSignInManager _signInManager;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            AppUserManager userManager,
            AppSignInManager signInManager,
            IOptions<AppSettings> appSettings,
            IEmailService emailService,
            IMapper mapper)
            : base(mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<List<UserModel>> GetAllFullAsync()
        {
            var users = await _userRepository.GetAllFullAsync();
            var result = users.Select(MapToUserModel).ToList();
            return result;
        }

        public async Task<UserModel> GetByIdFullAsync(int id)
        {
            var user = await _userRepository.GetByIdFullAsync(id);
            var result = MapToUserModel(user);
            return result;
        }

        public async Task<UserModel> CreateUserAsync(NewUserModel model)
        {
            var user = new AppUser()
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Description = model.Description,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                Enabled = true,
                ProjectId = model.ProjectId,
                RouteIds = model.RouteIds
            };

            var role = await _roleRepository.GetByIdAsync(model.RoleId);
            CheckRoleData(user, role);

            var userResult = await _userManager.CreateAsync(user, model.Password);

            if (userResult.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, role.NormalizedName);

                if (!roleResult.Succeeded)
                {
                    ThrowIdentityError(roleResult.Errors);
                }

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Регистрация в системе ЦОДД",
                    GetNewUserEmail(user, model.Password));
            }
            else
            {
                ThrowIdentityError(userResult.Errors);
            }

            var result = await GetByIdFullAsync(user.Id);
            return result;
        }

        public async Task<UserModel> ConfirmUserAsync(int userId, ConfirmUserModel model)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user.Status != UserStatusEnum.WaitForConfirmation)
            {
                throw new InvalidOperationException("Invalid user status. Should be wait for confirmation");
            }

            user.ProjectId = model.ProjectId;
            user.RouteIds = model.RouteIds;
            user.EmailConfirmed = true;
            user.Enabled = true;

            var role = await _roleRepository.GetByIdAsync(model.RoleId);

            CheckRoleData(user, role);

            var roleResult = await _userManager.AddToRoleAsync(user, role.NormalizedName);

            if (!roleResult.Succeeded)
            {
                ThrowIdentityError(roleResult.Errors);
            }

            await _userRepository.UpdateAsync(user, true);

            await _emailService.SendEmailAsync(
                user.Email,
                "Ваша учётная запись успешно подтверждёна",
                GetConfirmEmailBody());

            var result = await GetByIdFullAsync(userId);
            return result;
        }

        public async Task ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var passwordResult = await _userManager.ChangePasswordAsync(user, newPassword);

            if (passwordResult.Succeeded)
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Ваш пароль был изменён",
                    GetChangePasswordEmailBody(user, newPassword));
            }
            else
            {
                ThrowIdentityError(passwordResult.Errors);
            }
        }

        public async Task<UserModel> ToggleUserStatusAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user.Status == UserStatusEnum.Active)
            {
                //Disable if active
                user.Enabled = false;
            }
            else if (user.Status == UserStatusEnum.Disabled || user.Status == UserStatusEnum.Locked)
            {
                //Enable if disabled
                user.Enabled = true;
                user.LockoutEnd = null;
                user.AccessFailedCount = 0;
            }
            else
            {
                throw new InvalidOperationException("Incorrect user status. Should be Active, Disabled or Locked");
            }

            await _userRepository.UpdateAsync(user, true);

            var result = await GetByIdFullAsync(user.Id);
            return result;
        }

        public async Task<List<RoleModel>> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllAsync();
            var result = roles.Select(r => _mapper.Map<RoleModel>(r)).ToList();
            return result;
        }

        public Task<List<UserStatusModel>> GetAllStatuses()
        {
            var result = Enum.GetValues(typeof(UserStatusEnum))
                .Cast<UserStatusEnum>()
                .Select(x => new UserStatusModel(x))
                .ToList();

            return Task.FromResult(result);
        }

        private string GetNewUserEmail(AppUser user, string password)
        {
            var result = $@"
Вы были успешно зарегистрированы в системе ""ЦОДД Администратор"" <br/> <br/>

Для входа в систему используйте: <br/> <br/>

<b>Email:</b> {user.Email} <br/>
<b>Пароль:</b> {password} <br/> <br/>

<span style=""color: red"">Уведомляем Вас, что передача своего логина и пароля 
третьим лицам категорически запрещена.</span>
";

            return result;
        }

        private UserModel MapToUserModel(AppUser user)
        {
            var userModel = MapToModel(user);

            var role = user.UserRoles.FirstOrDefault()?.Role;

            if (role != null)
            {
                userModel.Role = _mapper.Map<RoleModel>(role);
            }

            return userModel;
        }

        private string GetConfirmEmailBody()
        {
            return $@"Уважаемый пользователь, <br/> <br/>
Ваша учётная запись была подтверждена администратором. Используйте ваш e-mail
и пароль для входа в систему - <a href=""{_appSettings.BaseSiteUrl}"">ЦОДД Администратор</a>.
";
        }

        private string GetChangePasswordEmailBody(AppUser user, string password)
        {
            var result = $@"
Ваш пароль был измененён администратором. <br/> <br/>

Для входа в систему используйте: <br/> <br/>

<b>Email:</b> {user.Email} <br/>
<b>Пароль:</b> {password} <br/> <br/>

<span style=""color: red"">Уведомляем Вас, что передача своего логина и пароля 
третьим лицам категорически запрещена.</span>
";

            return result;
        }

        private void CheckRoleData(AppUser user, AppRole role)
        {
            if (role.Name == RoleNames.Transporter && !user.ProjectId.HasValue)
            {
                throw new InvalidOperationException("TransporterId is required");
            }

            if (role.Name == RoleNames.Mechanic && 
                (!user.ProjectId.HasValue || user.RouteIds == null || !user.RouteIds.Any()))
            {
                throw new InvalidOperationException("RouteIds is required");
            }
        }

        private void ThrowIdentityError(IEnumerable<IdentityError> identityErrors)
        {
            var message = string.Join(". ", identityErrors.Select(x => x.Description));
            throw new InvalidOperationException(message);
        }
    }
}
