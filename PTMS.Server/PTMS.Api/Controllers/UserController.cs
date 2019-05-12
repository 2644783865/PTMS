using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    [PtmsAuthorizeAdmin]
    public class UserController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("/users")]
        public async Task<ActionResult<List<UserModel>>> GetAll()
        {
            var result = await _userService.GetAllWithRolesAsync();
            return result;
        }

        [HttpGet("/user/{id}")]
        public async Task<ActionResult<UserModel>> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return result;
        }

        [HttpPost("/user/{id}/confirm")]
        public async Task<ActionResult<UserModel>> GetAll(int id, [FromBody]ConfirmUserModel model)
        {
            var result = await _userService.ConfirmUserAsync(id, model);
            return result;
        }

        [HttpPost("/user")]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody]NewUserModel model)
        {
            var result = await _userService.CreateUserAsync(model);
            return result;
        }

        [HttpPost("/user/{id}/changePassword")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody]ChangePasswordModel model)
        {
            await _userService.ChangePasswordAsync(id, model.Password);
            return Ok();
        }

        [HttpPost("/user/{id}/toggle")]
        public async Task<ActionResult<UserModel>> ToggleUserStatus(int id)
        {
            var result = await _userService.ToggleUserStatusAsync(id);
            return result;
        }

        [HttpGet("/roles")]
        public async Task<ActionResult<List<RoleModel>>> GetAllRoles()
        {
            var result = await _userService.GetAllRoles();
            return result;
        }

        [HttpGet("/userStatuses")]
        public async Task<ActionResult<List<UserStatusModel>>> GetAllStatuses()
        {
            var result = await _userService.GetAllStatuses();
            return result;
        }
    }
}
