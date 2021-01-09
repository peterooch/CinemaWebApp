using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class HallVM
    {
        public List<int> seats;
        public string hall;
        public string movie;
        public DateTime start;
        public DateTime end;
        public HallVM(Screening screen)
        {
            seats = screen.Taken.Select(t => t.Seat).ToList();
            hall = screen.HallID;
            movie = screen.Movie.Name;
            start = screen.StartTime;
            end = screen.EndTime;
        }
    }
}
