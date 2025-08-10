using DHouse.Core.Domain.Entities;
using DHouse.Core.Infrastructure.Raven.Indexes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Security.Claims;

namespace DHouse.Core.API.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthController : Controller
{
    private readonly IAsyncDocumentSession _session;
    private readonly IPasswordHasher<Usuario> _hasher;
    public AuthController(IAsyncDocumentSession s, IPasswordHasher<Usuario> h) { _session = s; _hasher = h; }

    [HttpGet("/admin/auth/login")]
    public IActionResult Login() => View();

    [ValidateAntiForgeryToken]
    [HttpPost("/admin/auth/login")]
    public async Task<IActionResult> LoginPost(string email, string senha)
    {
        var user = await _session.Query<Usuario, Usuarios_PorEmail>()
            .Where(u => u.Email == email.ToLower())
            .FirstOrDefaultAsync();

        if (user is null) { ModelState.AddModelError("", "Credenciais inválidas."); return View("Login"); }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, senha);
        if (result == PasswordVerificationResult.Failed)
        { ModelState.AddModelError("", "Credenciais inválidas."); return View("Login"); }

        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.Nome),
            new(ClaimTypes.Email, user.Email)
        };
        foreach (var r in user.Roles) claims.Add(new Claim(ClaimTypes.Role, r));

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        return Redirect("/admin");
    }

    [HttpPost("/admin/auth/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/admin/auth/login");
    }

    [HttpGet("/admin/auth/forbidden")]
    public IActionResult Forbidden() => Content("Acesso negado.");
}
