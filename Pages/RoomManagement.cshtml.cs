using Class_Management.Data;
using Class_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Globalization;
using System.IO;

namespace Class_Management.Pages
{
    public class RoomManagementModel : PageModel
    {
        private readonly AppDbContext _context;

        public RoomManagementModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room NewRoom { get; set; }

        public List<Room> Rooms { get; set; }
        public string Message { get; set; }

        public async Task OnGetAsync()
        {
            Rooms = await _context.Rooms.ToListAsync();
        }
        public IActionResult OnPostExportAll()
        {
            var rooms = _context.Rooms.ToList();

            if (rooms.Count == 0)
                return Content("No rooms available.");

            using var workbook = new XLWorkbook();

            foreach (var room in rooms)
            {
                var ws = workbook.Worksheets.Add(room.RoomName);

                ws.Cell(1, 1).Value = room.RoomName;
                ws.Range(1, 1, 1, 5).Merge();
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;


                string[] headers = { "Time", "Day", "Subject", "Instructor", "Section" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(2, i + 1).Value = headers[i];
                    ws.Cell(2, i + 1).Style.Fill.BackgroundColor = XLColor.LightGreen;
                    ws.Cell(2, i + 1).Style.Font.Bold = true;
                    ws.Cell(2, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                int row = 3;

                var allSchedules = new List<dynamic>();

                var days = new List<(string dayName, IEnumerable<dynamic> schedules)>
                {
                    ("Monday", _context.ScheduleMonday.ToList()),
                    ("Tuesday", _context.ScheduleTuesday.ToList()),
                    ("Wednesday", _context.ScheduleWednesday.ToList()),
                    ("Thursday", _context.ScheduleThursday.ToList()),
                    ("Friday", _context.ScheduleFriday.ToList()),
                    ("Saturday", _context.ScheduleSaturday.ToList())
                };

                foreach (var (dayName, schedules) in days)
                {
                    allSchedules.AddRange(
                        schedules
                            .Where(s => s.Room == room.RoomName)
                            .OrderBy(s => s.Start)
                            .Select(s => new
                            {
                                s.Start,
                                s.End,
                                s.Day,
                                s.Subject,
                                s.Instructor,
                                s.Section
                            })
                    );
                }

                foreach (var s in allSchedules)
                {
                    ws.Cell(row, 1).Value = $"{s.Start:hh\\:mm} - {s.End:hh\\:mm}";
                    ws.Cell(row, 2).Value = s.Day;
                    ws.Cell(row, 3).Value = s.Subject;
                    ws.Cell(row, 4).Value = s.Instructor;
                    ws.Cell(row, 5).Value = s.Section;
                    row++;
                }

                ws.Columns().AdjustToContents();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var now = DateTime.Now;
            string formattedDate = now.ToString("yyyy_MMMM_dd", CultureInfo.InvariantCulture);
            string fileName = $"export_rooms_schedules_sti-alaminos_{formattedDate}.xlsx";

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(NewRoom.RoomName) || string.IsNullOrWhiteSpace(NewRoom.Type))
            {
                Message = "Please fill in all fields correctly.";
                Rooms = await _context.Rooms.ToListAsync();
                return Page();
            }

            _context.Rooms.Add(NewRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage(); // Reload page to show updated table
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                Message = "Room not found.";
                return RedirectToPage();
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return RedirectToPage(); // Refresh page
        }

    }
}
