using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(ILogger<HomeController> logger,
            IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<bool> AddRoles()
        {
            return await _roleService.AddRoles();
        }
    }
}
