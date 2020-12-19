using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaWebApp.Models
{
    public class ScreeningsBag
    {
        private readonly Dictionary<DayOfWeek, IEnumerable<Screening>> screenings_by_day = new Dictionary<DayOfWeek, IEnumerable<Screening>>();
        public IEnumerable<Hall> Halls { get; }
        public string SelectedHall { get; }
        public ScreeningsBag(IEnumerable<Screening> screenings, IEnumerable<Hall> halls, string selected_hall)
        {
            Halls = halls;
            SelectedHall = selected_hall;

            DateTime first_date = screenings.Min(s => s.StartTime);
            DateTime last_date  = screenings.Max(s => s.StartTime);

            if (last_date - first_date > Screening.Week)
                throw new Exception("Range of items is bigger than a week");

            foreach (DayOfWeek day in DayIterator)
                screenings_by_day[day] = screenings.Where(s => s.StartTime.DayOfWeek == day).OrderBy(s => s.StartTime);
        }
        public IEnumerable<Screening> this[DayOfWeek day]
        {
            get 
            {
                return screenings_by_day[day];
            }
        }
        public IEnumerable<DayOfWeek> DayIterator
        {
            get
            {
                return Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            }
        }
    }
}
