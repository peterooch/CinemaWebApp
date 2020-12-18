using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaWebApp.Models
{
    public class Screening : IComparable<Screening>
    {
        public Hall Hall { get; set; }
        public Movie Movie { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Ticket> Taken { get; set; }
        [NotMapped]
        public DateTime EndTime { get => StartTime + Duration; }

        /* Check if there is an overlap with other screening object
           e.g. cant have overlapping screenings in the same hall */
        public bool Overlap(Screening s)
        {
            if (Hall.Id != s.Hall.Id)
                return false;

            /* TESTME */
            return StartTime <= s.EndTime && EndTime >= s.StartTime;
        }
        /* IComparable::CompareTo */
        public int CompareTo(Screening other)
        {
            return StartTime.CompareTo(other.StartTime);
        }
        public override bool Equals(object o)
        {
            if (o is Screening s)
                return Hall.Id == s.Hall.Id && StartTime == s.StartTime;
            return false;
        }
        public static bool operator ==(Screening lhs, Screening rhs) => lhs.Equals(rhs);
        public static bool operator !=(Screening lhs, Screening rhs) => !lhs.Equals(rhs);
        public override int GetHashCode() => base.GetHashCode();
    }
}
