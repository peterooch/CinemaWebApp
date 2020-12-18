using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DataContext context;
        public RegisterController(DataContext context)
        {
            this.context = context;
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
            return RedirectToAction("Index", "Home");
        }
    }
}
