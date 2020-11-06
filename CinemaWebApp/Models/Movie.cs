using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Movie
    {
        [Key]
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int MinAge { get; set; }
        public string Category { get; set; }

        public double TicketCost()
        {
            return CalcPrice(1);
        }
        public double CalcPrice(int ticket_count)
        {
            return (Price * (Discount / 100)) * ticket_count;
        }
    }
}
