using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace back_la.Controllers;

public class AccountController : Controller
{
  public async Task Login(string returnUrl = "/")
  {
    await HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties()
    {
      RedirectUri = returnUrl
    });
  }

  public async Task Logout()
  {
    await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties()
    {
      RedirectUri = Url.Action("Index", "Home")
    });

    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
  }

  public IActionResult Index()
  {
    return View();
  }
}