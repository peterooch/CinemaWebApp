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
    }
}
