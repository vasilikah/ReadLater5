using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReadLater5.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly string JwtTokenSecretKey = "#yt0$@nit@ryInf0rm@tion$y$tem#";
        private readonly int JwtTokenExpirationMinutes = 60000;
        private readonly string JwtTokenAudience = "ReadLater";
        private readonly string JwtTokenIssuer = "ReadLater";
        private readonly string JwtTokenCookieName = "ReadLaterAuthCookie";

        public UsersController(ILogger<HomeController> logger,
            IUserService userService,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password,
                    isPersistent: false, lockoutOnFailure: false);

                if (!loginResult.Succeeded)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByNameAsync(loginModel.Username);

                return Ok(GetToken(user));
            }
            return BadRequest(ModelState);
        }

        private string GetToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenSecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddMinutes(JwtTokenExpirationMinutes),
                audience: JwtTokenAudience,
                issuer: JwtTokenIssuer
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

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
