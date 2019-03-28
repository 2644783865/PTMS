using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.Account;
using PTMS.Domain.Entities;

namespace PTMS.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;

        public AccountController(
            IAccountService accountService,
            UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("/account/login")]
        public async Task<ActionResult<object>> Login([FromBody]LoginModel model)
        {
            var result = await _accountService.Login(model.Email, model.Password);
            return new { Token = result };
        }

        [AllowAnonymous]
        [HttpPost("/account/register")]
        public async Task<ActionResult<object>> Register([FromBody]RegisterModel model)
        {
            var result = await _accountService.Login(model.Email, model.Password);
            return new { Token = result };
        }

        [HttpGet("/account/identity")]
        public async Task<ActionResult<AccountIdentityModel>> GetIdentity()
        {
            var result = await _accountService.GetIdentityAsync(User);
            return result;
        }
    }
}