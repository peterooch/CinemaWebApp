using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CinemaWebApp.Models;
using CinemaWebApp.Data;
using Microsoft.AspNetCore.DataProtection;

namespace CinemaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext context;
        private readonly IDataProtector protector;
        public HomeController(DataContext context, IDataProtectionProvider provider)
        {
            this.context = context;
            SeedData.Init(context);
            protector = provider.SetCookieProtector();
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue(ExtensionMethods.UserCookie, out string cookie))
            {
                string email = protector.Unprotect(cookie);

                User user = context.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                    this.SetViewData(user);
            }
            HomeVM model = new HomeVM
            {
                movies = context.Movies.Select(m => m.Name).ToList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
