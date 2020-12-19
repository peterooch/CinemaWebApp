using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly DataContext context;

        public ScheduleController(DataContext context)
        {
            this.context = context;
        }
#nullable enable
        public IActionResult Index(string? weekstart, string? hall)
        {
            DateTime sunday;

            if (weekstart != null)
            { 
                sunday = DateTime.Parse(weekstart);
            }
            else
            {
                sunday = DateTime.Now.Date;
                sunday = sunday.AddDays(-(int)sunday.DayOfWeek);
            }

            IEnumerable<Hall> halls = context.Halls;
            string selected_hall;

            if (hall != null && halls.Any(h => h.Id == hall))
            {
                selected_hall = hall;
            }
            else
            {
                selected_hall = halls.First().Id;
            }

            IEnumerable<Screening> weekly_screenings = context.Screenings.Where(s => Fits(s, selected_hall, sunday));

            ScreeningsBag model = new ScreeningsBag(weekly_screenings, halls, selected_hall);

            ViewData["weekstart"] = sunday.ToString("dd-MM-yyyy");
            return View(model);
        }
        private static bool Fits(Screening s, string hall, DateTime sunday)
        {
            if (s.HallID != hall)
                return false;

            if (s.StartTime < sunday || s.StartTime >= (sunday + Screening.Week))
                return false;

            return true;
        }
    }
}
