using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public sealed class Cinema
    {
        private Cinema() 
        { 
            /* Do initialization stuff here */
        }

        private static readonly Lazy<Cinema> Singleton = new Lazy<Cinema>(() => new Cinema());

        public static Cinema Instance
        {
            get
            {
                return Singleton.Value;
            }
        }
    }
}
