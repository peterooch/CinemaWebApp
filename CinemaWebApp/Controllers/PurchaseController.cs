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
                .First(s => s.HallID == order.Hall && s.StartTime == start);

            List<Ticket> tickets = order.Seats.Select(seat => new Ticket(screening, seat)).ToList();
            int? current_order = HttpContext.Session.GetInt32("CurrentOrder");

            TicketOrder ticketOrder;

            int? loggeduser = HttpContext.Session.GetInt32("UserID");
            User user = context.Users.Find(loggeduser);

            var ticket_ctx = context.TicketOrders.Include(o => o.Buyer);
            if (ticket_ctx.Any(o => o.Buyer.UserID == loggeduser && o.Paid == false))
            {
                ticketOrder = ticket_ctx
                    .Include(to => to.Tickets)
                    .First(to => to.Buyer.UserID == loggeduser);
                ticketOrder.Tickets.AddRange(tickets);
                ticketOrder.Total += screening.Movie.CalcPrice(tickets.Count);
                context.TicketOrders.Update(ticketOrder);
            }
            else if (current_order != null) // Is this useless?
            {
                ticketOrder = ticket_ctx
                    .Include(to => to.Tickets)
                    .First(to => to.ID == current_order);
                ticketOrder.Tickets.AddRange(tickets);
                ticketOrder.Total += screening.Movie.CalcPrice(tickets.Count);
                context.TicketOrders.Update(ticketOrder);
            }
            else
            {
                ticketOrder = new TicketOrder
                {
                    Paid = false,
                    Total = screening.Movie.CalcPrice(tickets.Count),
                    Tickets = tickets,
                    Buyer = user
                };
                context.TicketOrders.Add(ticketOrder);
            }
            context.SaveChanges();
            HttpContext.Session.SetInt32("CurrentOrder", ticketOrder.ID);
            return "/Purchase/Cart";
        }
        private TicketOrder GetCurrentOrder()
        {
            int? ordid = HttpContext.Session.GetInt32("CurrentOrder");
            int? uid = HttpContext.Session.GetInt32("UserID");

            if (ordid == null && uid != null)
            {
                TicketOrder t = context.TicketOrders
                    .Include(to => to.Buyer)
                    .FirstOrDefault(to => to.Buyer.UserID == uid && to.Paid == false);
                if (t != null)
                {
                    HttpContext.Session.SetInt32("CurrentOrder", t.ID);
                    ordid = t.ID;
                }
            }
            TicketOrder order = context.TicketOrders
                .Include(o => o.Tickets)
                .ThenInclude(t => t.Screening)
                .ThenInclude(s => s.Movie)
                .FirstOrDefault(o => o.ID == ordid);

            return order;
        }
        private Dictionary<Screening, int> CurrentOrderDictionary()
        {
            TicketOrder order = GetCurrentOrder();

            Dictionary<Screening, int> model = order?.ToDictionary() ?? new Dictionary<Screening, int>();

            ViewData["Total"] = order?.Total ?? 0.0;
            ViewData["OrderID"] = order?.ID;
            return model;
        }
        public IActionResult Checkout()
        {
            this.GetViewData();
            return View(CurrentOrderDictionary());
        }
        public IActionResult Cart()
        {
            this.GetViewData();
            return View(CurrentOrderDictionary());
        }

        public IActionResult ThanksBuy()
        {
            TicketOrder current_order = GetCurrentOrder();

            if (current_order != null)
            {
                current_order.Paid = true;
                context.TicketOrders.Update(current_order);
                context.SaveChanges();
            }
            HttpContext.Session.Remove("CurrentOrder");
            return View();
        }
    }
}
