﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CinemaWebApp.Models;
using CinemaWebApp.Data;

namespace CinemaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext context;
        public HomeController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            SeedData.Init(context);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
