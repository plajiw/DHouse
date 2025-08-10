using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DHouse.Core.API.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "SomenteAdmin")]
public class HomeController : Controller
{
    [HttpGet("/admin")]
    public IActionResult Index() => View();
}
