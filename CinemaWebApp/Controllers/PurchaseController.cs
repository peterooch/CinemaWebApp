using CinemaWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

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
            public string Movie { get; set; }
            public string Hall { get; set; }
            public string Start { get; set; }
            public List<int> Seats { get; set; }
        }
        [HttpPost]
        public string AddOrder()
        {
            Order order = JsonSerializer.Deserialize<Order>(Request.Form["Model"]);

            return "OK";
        }
    }
}
