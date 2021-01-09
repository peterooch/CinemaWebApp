using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApp.Controllers
{

    public class SignInController : Controller
    {
        private readonly DataContext context;
        private readonly IDataProtector protector;

        public SignInController(DataContext context, IDataProtectionProvider provider)
        {
            this.context = context;
            protector = provider.SetCookieProtector();
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

            this.SetUserCookie(Email, protector);
            this.SetViewData(user);

            return RedirectToAction("Index", "Home");
        }
    }
}
