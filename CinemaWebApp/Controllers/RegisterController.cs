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
    public class RegisterController : Controller
    {
        private readonly DataContext context;
        private readonly IDataProtector protector;
        public RegisterController(DataContext context, IDataProtectionProvider provider)
        {
            this.context = context;
            protector = provider.SetCookieProtector();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser()
        {
            string FirsName  = Request.Form["firstname"];
            string LastName  = Request.Form["lastname"];
            string Email     = Request.Form["email"];
            string Password  = Request.Form["psw"];
            string Password2 = Request.Form["psw-repeat"];

            if (Password != Password2)
            {
                return RedirectToAction("Index");
            }
            if (context.Users.Any(u => u.Email == Email))
            {
                return RedirectToAction("Index");
            }      

            User user = new User
            {
                Email = Email,
                FirstName = FirsName,
                LastName = LastName
            };

            user.StorePassword(Password);
            context.Users.Add(user);
            context.SaveChanges();

            this.SetUserCookie(Email, protector);
            this.SetViewData(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
