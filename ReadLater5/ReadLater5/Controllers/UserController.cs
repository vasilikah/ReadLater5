using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<HomeController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAddEdit userModel)
        {
            if (!userModel.password.ToLower().Equals(userModel.confirmPassword.ToLower()))
            {
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }

            var isSuccess = await _userService.Create(userModel);
            if (isSuccess)
            {
                return RedirectToAction("GetUserAccounts");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAccounts()
        {
            List<IdentityUser> model = await _userService.GetUserAccounts();
            return View("Index", model);
        }

    }
}
