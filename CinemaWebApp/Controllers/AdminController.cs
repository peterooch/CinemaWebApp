using CinemaWebApp.Data;
using CinemaWebApp.Models;
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
        public IActionResult Halls()
        {
            return View(context.Halls);
        }
        public IActionResult ManageHall(string hallID)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            Hall hall = context.Halls.Find(hallID);

            ViewData["NewHall"] = hall == null;

            if (hall == null)
            {
                hall = new Hall
                {
                    Id = "New hall ID",
                    Seats = 0
                };
            }
            return View(hall);
        }
        [HttpPost]
        public IActionResult UpdateHall([FromForm]Hall hall)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            if (context.Halls.Any(h => h.Id == hall.Id))
                context.Halls.Update(hall);
            else
                context.Halls.Add(hall);

            context.SaveChanges();
            return RedirectToAction("Halls");
        }
    }
}
