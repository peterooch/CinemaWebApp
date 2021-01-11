using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class NewScreeningVM
    {
        public IQueryable<Movie> movies;
        public IQueryable<Hall>  halls;
    }
}
