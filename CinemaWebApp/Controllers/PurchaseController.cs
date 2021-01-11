using CinemaWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using CinemaWebApp.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly DataContext context;

        public PurchaseController(DataContext context)
        {
            this.context = context;
        }
        private class Order
        {
            public string Hall { get; set; }
            public string Start { get; set; }
            public List<int> Seats { get; set; }
        }
        [HttpPost]
        public string AddOrder()
        {
            Order order = JsonSerializer.Deserialize<Order>(Request.Form["Model"]);
            DateTime start = DateTime.ParseExact(order.Start, Screening.Format, CultureInfo.InvariantCulture);
            Screening screening = context.Screenings
                .Include(s => s.Movie)
                .FirstOrDefault(s => s.HallID == order.Hall && s.StartTime == start);

            List<Ticket> tickets = order.Seats.Select(seat => new Ticket(screening, seat)).ToList();
            TicketOrder ticket = new TicketOrder
            {
                Paid = false,
                Total = tickets.Count * screening.Movie.Price,
                Tickets = tickets,
                Buyer = null
            };
            string dest = "/Purchase/Checkout";
            int? loggeduser = HttpContext.Session.GetInt32("UserID");
            if (loggeduser != null)
            {
                User user = context.Users.Find(loggeduser);
                ticket.Buyer = user;
                dest = "/Purchase/Cart";
            }
            context.TicketOrders.Add(ticket);
            context.SaveChanges();
            HttpContext.Session.SetInt32("CurrentOrder", ticket.ID);
            return dest;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Cart()
        {
            Dictionary<Screening, int> model = new Dictionary<Screening, int>();
            int? ordid = HttpContext.Session.GetInt32("CurrentOrder");
            TicketOrder order = context.TicketOrders
                .Include(o => o.Tickets)
                .ThenInclude(t=> t.Screening)
                .ThenInclude(s => s.Movie)
                .FirstOrDefault(o => o.ID == ordid);
            if (order != null)
            {
                foreach (Screening s in order.Tickets.Select(t => t.Screening).Distinct())
                    model[s] = 0;
                foreach (Ticket t in order.Tickets)
                    model[t.Screening] += 1;
            }

            ViewData["Total"] = (order != null) ? order.Total:0;
            return View(model);
        }
    }
}
