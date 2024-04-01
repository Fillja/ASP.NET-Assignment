using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class SiteSettings : Controller
{
    public IActionResult ChangeTheme(string theme)
    {
        var option = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(60),
        };
        Response.Cookies.Append("ThemeMode", theme, option);
        return Ok();
    }
}
