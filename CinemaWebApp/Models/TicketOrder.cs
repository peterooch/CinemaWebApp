using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class TicketOrder
    {
        public int ID;
        public User buyer;
        public bool paid;
        public List<Ticket> tickets;
        public double total;
    }
}
