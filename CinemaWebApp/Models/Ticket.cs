using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; } /* This is just to make EF happy */
        public int Seat { get; set; }
        public Screening Screening { get; set; }

        public Ticket()
        {

        }
        public Ticket(Screening screening, int seat)
        {
            Screening = screening;
            Seat = seat;
        }
    }
}
