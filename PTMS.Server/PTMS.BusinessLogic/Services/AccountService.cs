using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Account;
using PTMS.Common;
using PTMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppUserManager _userManager;
        private readonly AppSignInManager _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public AccountService(
            AppUserManager userManager,
            AppSignInManager signInManager,
            IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        public async Task<string> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, true);

            if (result.Succeeded)
            {
                var user = _userManager.Users.SingleOrDefault(r => r.Email == email);
                var roles =  await _userManager.GetRolesAsync(user);

                return GenerateJwtToken(user, roles);
            }

            var message = result.IsLockedOut
                ? "Ваш аккаунт заблокирован. Пожалуйста, подождите 10 минут и попробуйте войти снова. Вы также можете сбросить пароль или обратиться к системному администратору."
                : "Неверный e-mail или пароль";

            throw new UnauthorizedAccessException(message);
        }

        public async Task Register(RegisterModel model)
        {
            var user = new AppUser()
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Description = model.Description,
                Email = model.Email,
                EmailConfirmed = false,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                Enabled = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Send Email to admin
                await _emailService.SendEmailAsync(
                    _appSettings.AdminRecipient,
                    "Новый пользователь зарегистрировался в системе",
                    GetAdminRegisterEmail(user));

                //Send email to the new user
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Регистрация в системе ЦОДД",
                    GetUserRegisterEmail(user, model.Password));
            }
            else
            {
                var message = string.Join(". ", result.Errors.Select(x => x.Description));
                throw new InvalidOperationException(message);
            }
        }

        public async Task<AccountIdentityModel> GetIdentityAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            var roles = await _userManager.GetRolesAsync(user);

            var result = new AccountIdentityModel
            {
                Id = user.Id,
                Role = roles.First(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProjectId = user.ProjectId,
                RouteIds = user.RouteIds
            };

            return result;
        }

        private string GetAdminRegisterEmail(AppUser user)
        {
            var adminEmail = $@"
Новый пользователь зарегистрировался в системе <br/> <br/>
<b>ФИО:</b> {user.LastName} {user.FirstName} {user.MiddleName} <br/>
<b>Описание:</b> {user.Description} <br/>
<b>Email:</b> {user.Email} <br/>
<b>Телефон:</b> {user.PhoneNumber} <br/> <br/>

Чтобы разрешить пользователю доступ в систему и назначить роль
<a href=""{_appSettings.BaseSiteUrl}/users?id={user.Id}"">перейдите по ссылке</a>
";

            return adminEmail;
        }

        private string GetUserRegisterEmail(AppUser user, string password)
        {
            var result = $@"
Спасибо за регистрацию в системе ""ЦОДД Администратор"" <br/> <br/>

На данный момент Ваша учётная запись <u>не активна</u>. Администратор проверит
ваши данные, при необходимости свяжется с Вами по телефону <u>{user.PhoneNumber}</u>
и активирует вашу учётную запись. <br/> <br/>

<u>После активации</u> для входа в систему используйте: <br/> <br/>

<b>Email:</b> {user.Email} <br/>
<b>Пароль:</b> {password} <br/> <br/>

<span style=""color: red"">Уведомляем Вас, что передача своего логина и пароля 
третьим лицам категорически запрещена.</span>
";

            return result;
        }

        private string GenerateJwtToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_appSettings.JwtExpireDays));

            var token = new JwtSecurityToken(
                _appSettings.JwtIssuer,
                _appSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
