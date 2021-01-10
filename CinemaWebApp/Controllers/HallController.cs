using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Controllers
{
    public class HallController : Controller
    {
        private readonly DataContext context;

        public HallController(DataContext context)
        {
            this.context = context;
        }
        public IActionResult Index(string hall, string time)
        {
            if (!context.Halls.Any(h => h.Id == hall))
            {
                //error
            }
            DateTime td = DateTime.ParseExact(time, Screening.Format, CultureInfo.InvariantCulture);
            Screening screening = context.Screenings.Include(s => s.Movie).Include(s => s.Taken).FirstOrDefault();

            if (screening is null)
            {
                // error
            }
            ViewData["seats"] = context.Halls.First(h => h.Id == hall).Seats;
            return View(new HallVM(screening));
        }
    }
}
