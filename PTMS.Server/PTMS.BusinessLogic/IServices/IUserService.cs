using PTMS.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllWithRolesAsync();
    }
}
