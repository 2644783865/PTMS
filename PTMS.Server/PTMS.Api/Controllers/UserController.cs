using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
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
    }
}
