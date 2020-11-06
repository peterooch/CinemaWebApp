using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Screening
    {
        public int HallNumber;
        public DateTime StartTime;
        public TimeSpan Duretion;
        public List<Ticket> Taken;
        public Movie movie;
    }
}
