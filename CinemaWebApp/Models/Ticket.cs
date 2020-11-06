﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Ticket
    {
        public List<int> Seats { get; set; }
        public Screening Screening { get; set; }
    }
}