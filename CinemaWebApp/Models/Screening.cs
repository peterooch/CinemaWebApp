using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Screening
    {
        public int HallNumber { get; set; }
        public Movie Movie { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Ticket> Taken { get; set; }
    }
}
