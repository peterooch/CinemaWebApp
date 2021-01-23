using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class TicketOrder
    {
        [Key]
        public int ID { get; set; }
        public User Buyer { get; set; }
        public bool Paid { get; set; }
        public List<Ticket> Tickets { get; set; }
        public double Total { get; set; }

        public Dictionary<Screening, int> ToDictionary()
        {
            Dictionary<Screening, int> dict = new Dictionary<Screening, int>();

            foreach (Screening s in Tickets.Select(t => t.Screening).Distinct())
                dict[s] = 0;
            foreach (Ticket t in Tickets)
                dict[t.Screening] += 1;

            return dict;
        }
    }
}
