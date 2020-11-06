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
    }
}
