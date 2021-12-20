using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace ReadLater5.Controllers
{
    [ApiController]
    [Authorize("ReadLaterAuthorizationPolicy", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    public class BookmarksExternalController : Controller
    {
        IBookmarkService _bookmarkService;
        public BookmarksExternalController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        public IActionResult Index()
        {
            var claimSub = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Sub))?.Value;
            var claimUniqueName = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.UniqueName))?.Value;

            List<Bookmark> model = _bookmarkService.GetBookmarks();
            return View("~/Views/Bookmarks/Index.cshtml", model);
        }
    }
}
