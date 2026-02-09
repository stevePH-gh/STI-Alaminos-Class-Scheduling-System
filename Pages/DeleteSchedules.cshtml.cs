using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Class_Management.Data;
using Class_Management.Models;
using ClosedXML.Excel;
using System.IO;
using System.Globalization;

namespace Class_Management.Pages
{
    public class DeleteSchedulesModel : PageModel
    {
        private readonly AppDbContext _context;

        public IActionResult OnPostExportAll()
        {
            var allSchedules = new Dictionary<string, List<dynamic>>()
    {
        { "Monday",    _context.ScheduleMonday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Tuesday",   _context.ScheduleTuesday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Wednesday", _context.ScheduleWednesday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Thursday",  _context.ScheduleThursday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Friday",    _context.ScheduleFriday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Saturday",  _context.ScheduleSaturday.OrderBy(s => s.StartTicks).ToList<dynamic>() }
    };

            using var workbook = new XLWorkbook();

            foreach (var day in allSchedules)
            {
                var ws = workbook.Worksheets.Add($"{day.Key} Schedule");

                // Title
                ws.Cell(1, 1).Value = $"{day.Key.ToUpper()} SCHEDULE";
                ws.Range(1, 1, 1, 6).Merge().Style
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                // Headers
                string[] headers = { "Time", "Instructor", "Subject", "Section", "Type", "Room" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(2, i + 1).Value = headers[i];
                    ws.Cell(2, i + 1).Style
                        .Font.SetBold()
                        .Fill.SetBackgroundColor(XLColor.LightGreen)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }

                // Rows
                int row = 3;
                foreach (var s in day.Value)
                {
                    ws.Cell(row, 1).Value = $"{s.Start:hh\\:mm} - {s.End:hh\\:mm}";
                    ws.Cell(row, 2).Value = s.Instructor;
                    ws.Cell(row, 3).Value = s.Subject;
                    ws.Cell(row, 4).Value = s.Section;
                    ws.Cell(row, 5).Value = s.Type;
                    ws.Cell(row, 6).Value = s.Room;
                    row++;
                }

                ws.Columns().AdjustToContents();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var contents = stream.ToArray();

            string fileName = $"sti_alaminos_full_schedule_{DateTime.Now:yyyy_MM_dd}.xlsx";

            return File(
                contents,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }


        public DeleteSchedulesModel(AppDbContext context)
        {
            _context = context;
        }

        // Lists for each day
        public List<ScheduleMonday> MondaySchedules { get; set; }
        public List<ScheduleTuesday> TuesdaySchedules { get; set; }
        public List<ScheduleWednesday> WednesdaySchedules { get; set; }
        public List<ScheduleThursday> ThursdaySchedules { get; set; }
        public List<ScheduleFriday> FridaySchedules { get; set; }
        public List<ScheduleSaturday> SaturdaySchedules { get; set; }

        public List<string> Days { get; set; } = new List<string>
        {
            "Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"
        };

        public void OnGet()
        {
            // Fetch all schedules into memory first, then sort by Start
            MondaySchedules = _context.ScheduleMonday.ToList().OrderBy(s => s.Start).ToList();
            TuesdaySchedules = _context.ScheduleTuesday.ToList().OrderBy(s => s.Start).ToList();
            WednesdaySchedules = _context.ScheduleWednesday.ToList().OrderBy(s => s.Start).ToList();
            ThursdaySchedules = _context.ScheduleThursday.ToList().OrderBy(s => s.Start).ToList();
            FridaySchedules = _context.ScheduleFriday.ToList().OrderBy(s => s.Start).ToList();
            SaturdaySchedules = _context.ScheduleSaturday.ToList().OrderBy(s => s.Start).ToList();
        }

        // Helper to get the right list by day
        public IEnumerable<dynamic> GetSchedulesByDay(string day)
        {
            return day switch
            {
                "Monday" => MondaySchedules,
                "Tuesday" => TuesdaySchedules,
                "Wednesday" => WednesdaySchedules,
                "Thursday" => ThursdaySchedules,
                "Friday" => FridaySchedules,
                "Saturday" => SaturdaySchedules,
                _ => new List<dynamic>()
            };
        }

        // Generic Delete handler
        public IActionResult OnPostDelete(string day, int id)
        {
            switch (day)
            {
                case "Monday":
                    var m = _context.ScheduleMonday.Find(id);
                    if (m != null) _context.ScheduleMonday.Remove(m);
                    break;
                case "Tuesday":
                    var t = _context.ScheduleTuesday.Find(id);
                    if (t != null) _context.ScheduleTuesday.Remove(t);
                    break;
                case "Wednesday":
                    var w = _context.ScheduleWednesday.Find(id);
                    if (w != null) _context.ScheduleWednesday.Remove(w);
                    break;
                case "Thursday":
                    var th = _context.ScheduleThursday.Find(id);
                    if (th != null) _context.ScheduleThursday.Remove(th);
                    break;
                case "Friday":
                    var f = _context.ScheduleFriday.Find(id);
                    if (f != null) _context.ScheduleFriday.Remove(f);
                    break;
                case "Saturday":
                    var s = _context.ScheduleSaturday.Find(id);
                    if (s != null) _context.ScheduleSaturday.Remove(s);
                    break;
            }

            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostExportAndDelete(List<string> daysToDelete)
        {
            // First export everything (same logic as ExportAll)
            var allSchedules = new Dictionary<string, List<dynamic>>()
    {
        { "Monday",    _context.ScheduleMonday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Tuesday",   _context.ScheduleTuesday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Wednesday", _context.ScheduleWednesday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Thursday",  _context.ScheduleThursday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Friday",    _context.ScheduleFriday.OrderBy(s => s.StartTicks).ToList<dynamic>() },
        { "Saturday",  _context.ScheduleSaturday.OrderBy(s => s.StartTicks).ToList<dynamic>() }
    };

            using var workbook = new XLWorkbook();

            foreach (var day in allSchedules)
            {
                var ws = workbook.Worksheets.Add($"{day.Key} Schedule");

                ws.Cell(1, 1).Value = $"{day.Key.ToUpper()} SCHEDULE";
                ws.Range(1, 1, 1, 6).Merge().Style
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightGray)
                    .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                string[] headers = { "Time", "Instructor", "Subject", "Section", "Type", "Room" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(2, i + 1).Value = headers[i];
                    ws.Cell(2, i + 1).Style
                        .Font.SetBold()
                        .Fill.SetBackgroundColor(XLColor.LightGreen)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                }

                int row = 3;
                foreach (var s in day.Value)
                {
                    ws.Cell(row, 1).Value = $"{s.Start:hh\\:mm} - {s.End:hh\\:mm}";
                    ws.Cell(row, 2).Value = s.Instructor;
                    ws.Cell(row, 3).Value = s.Subject;
                    ws.Cell(row, 4).Value = s.Section;
                    ws.Cell(row, 5).Value = s.Type;
                    ws.Cell(row, 6).Value = s.Room;
                    row++;
                }

                ws.Columns().AdjustToContents();
            }

            // Write to memory stream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var fileBytes = stream.ToArray();

            // DELETE selected days
            if (daysToDelete != null)
            {
                foreach (var day in daysToDelete)
                {
                    switch (day)
                    {
                        case "Monday":
                            _context.ScheduleMonday.RemoveRange(_context.ScheduleMonday);
                            break;
                        case "Tuesday":
                            _context.ScheduleTuesday.RemoveRange(_context.ScheduleTuesday);
                            break;
                        case "Wednesday":
                            _context.ScheduleWednesday.RemoveRange(_context.ScheduleWednesday);
                            break;
                        case "Thursday":
                            _context.ScheduleThursday.RemoveRange(_context.ScheduleThursday);
                            break;
                        case "Friday":
                            _context.ScheduleFriday.RemoveRange(_context.ScheduleFriday);
                            break;
                        case "Saturday":
                            _context.ScheduleSaturday.RemoveRange(_context.ScheduleSaturday);
                            break;
                    }
                }

                _context.SaveChanges();
            }

            string fileName = $"sti_alaminos_export_delete_{DateTime.Now:yyyy_MM_dd}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

    }
}
