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

        public IActionResult Index(string cat, string age = "0")
        {
            if (HttpContext.Request.Cookies.TryGetValue(ExtensionMethods.UserCookie, out string cookie))
            {
                string email = protector.Unprotect(cookie);

                User user = context.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                    this.SetViewData(user);
            }

            if (!uint.TryParse(age, out uint i_age))
                i_age = 0;
            IQueryable<Movie> movies = context.Movies.Where(m => m.MinAge >= i_age);
            if (movies.Any(m => m.Category == cat))
                movies = movies.Where(m => m.Category == cat);

            HomeVM model = new HomeVM
            {
                movies = movies,
                categories = context.Movies.Select(m => m.Category).Distinct(),
                ages = context.Movies.Select(a => a.MinAge).Distinct().ToList()
            };
            model.ages.Sort();
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
