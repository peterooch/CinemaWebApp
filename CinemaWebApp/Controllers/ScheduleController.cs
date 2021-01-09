using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
#nullable disable
        {
            this.GetViewData();

            DateTime sunday = (weekstart != null) ? DateTime.Parse(weekstart) : DateTime.Now.Date;
            sunday = sunday.AddDays(-(int)sunday.DayOfWeek);
            ViewData["weekstart"] = sunday.ToString("dd-MM-yyyy");

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

            IQueryable<Screening> weekly_screenings = context.Screenings
                .Where(s => s.HallID == selected_hall && s.StartTime >= sunday && s.StartTime < (sunday + Screening.Week))
                .Include(s => s.Movie); // IMPORTANT!

            ScreeningsBag model = new ScreeningsBag(weekly_screenings, halls, selected_hall);

            return View(model);
        }
    }
}
