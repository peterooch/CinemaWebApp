using CinemaWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext context;
        public AdminController(DataContext context)
        {
            this.context = context;
        }
        public IActionResult ManageScreenings(string hall, string time)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");
            return View();
        }
        public IActionResult ManageMovies(string name)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult ManageHalls(string hall)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}
