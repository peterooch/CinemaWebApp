using CinemaWebApp.Data;
using CinemaWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext context;

        public AccountController(DataContext context)
        {
            this.context = context;
        }
        private User GetCurrentUser()
        {
            int? uid = HttpContext.Session.GetInt32("UserID");
            return context.Users.Find(uid);
        }
        public IActionResult Index()
        {
            this.GetViewData();

            User user = GetCurrentUser();

            if (user == null)
                RedirectToAction("Index", "Home");

            return View(user);
        }
        public IActionResult History()
        {
            this.GetViewData();

            User user = GetCurrentUser();
            if (user == null)
                RedirectToAction("Index", "Home");

            IQueryable<TicketOrder> orders = context.TicketOrders
                .Include(o => o.Buyer)
                .Include(o => o.Tickets)
                .ThenInclude(t => t.Screening)
                .ThenInclude(s => s.Movie)
                .Where(o => o.Buyer.UserID == user.UserID && o.Paid);

            Dictionary<int, Dictionary<Screening, int>> history =
                orders.ToDictionary(o => o.ID, o => o.ToDictionary());
            return View(history);
        }
        public IActionResult Update()
        {
            this.GetViewData();

            User user = GetCurrentUser();
            if (user == null)
                RedirectToAction("Index", "Home");

            return View(user);
        }
    }
}
