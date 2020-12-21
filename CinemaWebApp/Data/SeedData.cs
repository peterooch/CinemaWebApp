using CinemaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Data
{
    public static class SeedData
    {
        public static void Init(DataContext context)
        {
            List<Hall> halls = new List<Hall>()
            {
                new Hall() { Id = "1",    Seats = 200 },
                new Hall() { Id = "2",    Seats = 200 },
                new Hall() { Id = "3",    Seats = 200 },
                new Hall() { Id = "VIP1", Seats = 100 },
                new Hall() { Id = "VIP2", Seats = 100 }
            };

            if (!context.Halls.Any())
                context.Halls.AddRange(halls);

            DateTime current_week_start = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);

            List<Movie> the_movies = new List<Movie>()
            {
                new Movie("Spaceballs", "Comedy", 39.90, new TimeSpan(2,0,0), 10),
                new Movie("Breakdance 2: Electric Boogaloo", "Comedy", 19.90, new TimeSpan(1,45,0)),
                new Movie("The Big Lebowski", "Comedy", 24.90, new TimeSpan(1,50,0), 18),
                /* Need to think about more! */
            };

            IEnumerable<string> names = the_movies.Select(m => m.Name);

            if (!context.Movies.Any())
            {
                context.Movies.AddRange(the_movies);
                context.SaveChanges();
            }

            if (context.Screenings.Any())
                return;

            List<Movie> movies = context.Movies.Where(m => names.Any(n => n == m.Name)).ToList();

            List<Screening> screenings = new List<Screening>()
            {
                new Screening()
                {
                    Movie = movies[0],
                    HallID = "1",
                    StartTime = current_week_start.AddDays(1).AddHours(14)
                },
                new Screening()
                {
                    Movie = movies[0],
                    HallID = "1",
                    StartTime = current_week_start.AddDays(2).AddHours(14)
                },
                new Screening()
                {
                    Movie = movies[1],
                    HallID = "2",
                    StartTime = current_week_start.AddDays(1).AddHours(14)
                },
                new Screening()
                {
                    Movie = movies[1],
                    HallID = "2",
                    StartTime = current_week_start.AddDays(1).AddHours(17)
                },
                new Screening()
                {
                    Movie = movies[1],
                    HallID = "2",
                    StartTime = current_week_start.AddDays(2).AddHours(14)
                },
                new Screening()
                {
                    Movie = movies[2],
                    HallID = "VIP1",
                    StartTime = current_week_start.AddDays(4).AddHours(14)
                }
            };


            for (int i = 0; i < 12; i++)
            {
                foreach (Screening screening in screenings)
                {
                    Screening s = new Screening()
                    {
                        Movie = screening.Movie,
                        HallID = screening.HallID,
                        StartTime = screening.StartTime.AddDays(i * 7)
                    };
                    context.Screenings.Add(s); 
                    context.SaveChanges();
                }
            }
        }

    }
}
