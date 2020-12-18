using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApp.Controllers
{

    public class SignInController : Controller
    {
        private readonly DataContext context;
        public SignInController(DataContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login()
        {
            string Email = Request.Form["email"];
            string Password = Request.Form["psw"];
            User user;
            try
            {
                user = context.Users.Single(user => user.Email == Email);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("Index");
            }

            if (!user.VerifyPassword(Password))
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
