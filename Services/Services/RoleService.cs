using Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Services
{
    public class RoleService : IRoleService
    {
        private ReadLaterDataContext _ReadLaterDataContext;

        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(ReadLaterDataContext readLaterDataContext,
            RoleManager<IdentityRole> roleManager)
        {
            _ReadLaterDataContext = readLaterDataContext;

            _roleManager = roleManager;
        }

        public async Task<bool> AddRoles()
        {
            var role = new IdentityRole
            {
                Name = "Visitor"
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return false;
            }
            role = new IdentityRole
            {
                Name = "Admin"
            };
            result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }

    }
}
