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
        public string Image { get; set; }

        public Movie() { }

        public Movie(string Name, string Category, double Price, TimeSpan Duration, int MinAge = 0, int Discount = 0)
        {
            this.Name     = Name;
            this.Category = Category;
            this.Price    = Price;
            this.Duration = Duration;
            this.MinAge   = MinAge;
            this.Discount = Discount;
        }
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
