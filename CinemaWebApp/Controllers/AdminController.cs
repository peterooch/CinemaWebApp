using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext context;
        public AdminController(DataContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult Movies()
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            return View(context.Movies);
        }
        public IActionResult EditMovie(string name)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            bool exists = context.Movies.Any(m => m.Name == name);
            ViewData["NewMovie"] = !exists;

            if (exists)
                return View(context.Movies.Find(name));

            Movie model = new Movie
            {
                Name = "New Movie",
                Price = 39.90,
                Category = "Action",
                Discount = 0,
                Duration = new TimeSpan(1, 30, 0),
                MinAge = 0
            };
            return View(model);
        }
        public class MovieForm : Movie
        {
            public IFormFile Poster { get; set; }
        }
        [HttpPost]
        public IActionResult UpdateMovie([FromForm]MovieForm form)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            if (context.Movies.Any(m => m.Name == form.Name))
                context.Movies.Update(form);
            else
                context.Movies.Add(form);

            context.SaveChanges();
            
            if (form.Poster != null)
            {
                string dest_path = $"wwwroot/PIC/{form.Name}.jpg";

                if (System.IO.File.Exists(dest_path))
                    System.IO.File.Delete(dest_path);

                using FileStream destStream = System.IO.File.OpenWrite(dest_path);
                using Stream srcStream = form.Poster.OpenReadStream();

                srcStream.CopyTo(destStream);
            }
            return RedirectToAction("Movies");
        }
        public IActionResult NewScreening()
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            NewScreeningVM viewmodel = new NewScreeningVM
            {
                movies = context.Movies,
                halls = context.Halls
            };
            return View(viewmodel);
        }
        public class ScreeningForm
        {
            public string MovieName { get; set; }
            public string Hall { get; set; }
            public DateTime StartDay { get; set; }
            public TimeSpan StartHour { get; set; }
        }
        [HttpPost]
        public IActionResult AddScreening([FromForm]ScreeningForm form)
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

            DateTime StartTime = form.StartDay + form.StartHour;
            Movie movie = context.Movies.Find(form.MovieName);

            if (movie == null || !context.Halls.Any(h => h.Id == form.Hall))
                return RedirectToAction("Index");

            Screening screening = new Screening
            {
                HallID = form.Hall,
                Movie = movie,
                StartTime = StartTime,
            };
            if (context.Screenings
                .Where(s => s.HallID == screening.HallID && s.StartTime.Date == screening.StartTime.Date)
                .Include(s => s.Movie)
                .ToList()
                .Any(s => s.Overlap(screening)))
            {
                return RedirectToAction("Index");
            }
            context.Screenings.Add(screening);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Halls()
        {
            if (!this.IsAdmin(context))
                return RedirectToAction("Index", "Home");

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
