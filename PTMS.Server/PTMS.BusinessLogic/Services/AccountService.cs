using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PTMS.BusinessLogic.Config;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Account;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppSettings _jwtConfig;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<AppSettings> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig.Value;
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

            var message = result.IsLockedOut || result.IsNotAllowed
                ? "Ваш аккаунт заблокирован. Пожалуйста, подождите 10 минут и попробуйте войти снова. Вы также можете сбросить пароль или обратиться к системному администратору."
                : "Неверный e-mail или пароль";

            throw new UnauthorizedAccessException(message);
        }

        public async Task Register(RegisterModel model)
        {
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Todo - send email to administrators
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
                Roles = roles,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return result;
        }

        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfig.JwtExpireDays));

            var token = new JwtSecurityToken(
                _jwtConfig.JwtIssuer,
                _jwtConfig.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
