using Class_Management.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Class_Management.Pages
{
    public class OverviewModel : PageModel
    {
        private readonly AppDbContext _context;

        public OverviewModel(AppDbContext context)
        {
            _context = context;
        }

        public List<string> Days { get; set; } = new List<string>
        {
            "Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"
        };

        public Dictionary<string, List<dynamic>> SchedulesByDay { get; set; } = new();
        public Dictionary<string, List<string>> RoomsPerDay { get; set; } = new();
        public Dictionary<string, List<string>> InstructorsPerDay { get; set; } = new();
        public Dictionary<string, List<string>> SectionsPerDay { get; set; } = new();
        public List<string> AllSections { get; set; } = new();

        public void OnGet()
        {
            // Load schedules grouped by day
            SchedulesByDay["Monday"] = _context.ScheduleMonday.OrderBy(s => s.StartTicks).Cast<dynamic>().ToList();
            SchedulesByDay["Tuesday"] = _context.ScheduleTuesday.OrderBy(s => s.StartTicks).Cast<dynamic>().ToList();
            SchedulesByDay["Wednesday"] = _context.ScheduleWednesday.OrderBy(s => s.StartTicks).Cast<dynamic>().ToList();
            SchedulesByDay["Thursday"] = _context.ScheduleThursday.OrderBy(s => s.StartTicks).Cast<dynamic>().ToList();
            SchedulesByDay["Friday"] = _context.ScheduleFriday.OrderBy(s => s.StartTicks).Cast<dynamic>().ToList();
            SchedulesByDay["Saturday"] = _context.ScheduleSaturday.OrderBy(s => s.StartTicks).Cast<dynamic>().ToList();

            // Extract Rooms / Instructors / Sections per day
            foreach (var day in Days)
            {
                var list = SchedulesByDay[day];

                RoomsPerDay[day] = list.Select(s => (string)s.Room).Distinct().ToList();
                InstructorsPerDay[day] = list.Select(s => (string)s.Instructor).Distinct().ToList();
                SectionsPerDay[day] = list.Select(s => (string)s.Section).Distinct().ToList();
            }

            // ⭐ global section list across all days (ALWAYS complete)
            AllSections = SectionsPerDay
                .SelectMany(x => x.Value)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

    }
}
