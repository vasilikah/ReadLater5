using Entity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<bool> Create(UserAddEdit userModel);
        Task<List<IdentityUser>> GetUserAccounts();
    }
}
