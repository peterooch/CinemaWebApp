using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class Hall
    {
        [Key]
        public string Id { get; set; }
        public int Seats { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}
