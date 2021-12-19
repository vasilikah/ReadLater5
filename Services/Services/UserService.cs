using Data;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private ReadLaterDataContext _ReadLaterDataContext;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(ReadLaterDataContext readLaterDataContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _ReadLaterDataContext = readLaterDataContext;

            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Create(UserAddEdit userModel)
        {
            var user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email
            };
            var password = userModel.password;

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return false;
            }

            var role = await _roleManager.FindByNameAsync("Visitor");
            if (role == null)
            {
                role = new IdentityRole
                {
                    Name = "Visitor"
                };
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, role.Name);
            return true;
        }

        public async Task<List<IdentityUser>> GetUserAccounts()
        {
            return await _ReadLaterDataContext.Users.ToListAsync();
        }

    }
}
