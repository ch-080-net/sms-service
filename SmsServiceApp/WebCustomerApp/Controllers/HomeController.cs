using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebApp.Models;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        public IActionResult Index()
        {
            return View();
        }
        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = _localizer["Your contact page."];

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl, string returnUrlForLanguageSwitch)
        {
            Response.Cookies.Append(
         CookieRequestCultureProvider.DefaultCookieName,
         CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
         new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
     );

            return LocalRedirect(returnUrl);
        }


    }
}