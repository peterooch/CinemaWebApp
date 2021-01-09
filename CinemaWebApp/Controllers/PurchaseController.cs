using CinemaWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly DataContext context;

        public PurchaseController(DataContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public IActionResult Index(string model)
        {
            
            return View();
        }
    }
}
