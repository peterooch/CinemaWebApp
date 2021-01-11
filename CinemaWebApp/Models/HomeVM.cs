using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class HomeVM
    {
        public IEnumerable<Movie> movies;
        public IEnumerable<string> categories;
        public List<int> ages;
    }
}
