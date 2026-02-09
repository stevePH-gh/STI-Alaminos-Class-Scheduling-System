using Class_Management.Data;
using Class_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using System.Globalization;

namespace Class_Management.Pages
{
    public class InstructorLoadingModel : PageModel
    {
        private readonly AppDbContext _context;

        public InstructorLoadingModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Instructor NewInstructor { get; set; }

        public List<string> Types { get; set; } = new List<string>();

        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        public string Message { get; set; }

        public void OnGet()
        {
            Types = _context.Courses.Select(c => c.Name).ToList();
            Instructors = _context.Instructors.ToList();
        }
        public IActionResult OnPostExportAll()
        {
            var instructors = _context.Instructors.ToList();

            if (instructors.Count == 0)
                return Content("No instructors available.");

            using var workbook = new XLWorkbook();

            foreach (var instructor in instructors)
            {
                var ws = workbook.Worksheets.Add($"{instructor.FirstName}_{instructor.LastName}");

                ws.Cell(1, 1).Value = $"{instructor.FirstName} {instructor.LastName}";
                ws.Range(1, 1, 1, 4).Merge();
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                string[] headers = { "Time", "Day", "Subject", "Room" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(2, i + 1).Value = headers[i];
                    ws.Cell(2, i + 1).Style.Fill.BackgroundColor = XLColor.LightGreen;
                    ws.Cell(2, i + 1).Style.Font.Bold = true;
                    ws.Cell(2, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                int row = 3;

                var allSchedules = new List<dynamic>();

                // Monday
                allSchedules.AddRange(
                    _context.ScheduleMonday
                        .Where(s => s.Instructor == instructor.FirstName + " " + instructor.LastName)
                        .ToList() // fetch into memory
                        .OrderBy(s => s.Start) // order by Start
                );

                // Tuesday
                allSchedules.AddRange(
                    _context.ScheduleTuesday
                        .Where(s => s.Instructor == instructor.FirstName + " " + instructor.LastName)
                        .ToList()
                        .OrderBy(s => s.Start)
                );

                // Wednesday
                allSchedules.AddRange(
                    _context.ScheduleWednesday
                        .Where(s => s.Instructor == instructor.FirstName + " " + instructor.LastName)
                        .ToList()
                        .OrderBy(s => s.Start)
                );

                // Thursday
                allSchedules.AddRange(
                    _context.ScheduleThursday
                        .Where(s => s.Instructor == instructor.FirstName + " " + instructor.LastName)
                        .ToList()
                        .OrderBy(s => s.Start)
                );

                // Friday
                allSchedules.AddRange(
                    _context.ScheduleFriday
                        .Where(s => s.Instructor == instructor.FirstName + " " + instructor.LastName)
                        .ToList()
                        .OrderBy(s => s.Start)
                );

                // Saturday
                allSchedules.AddRange(
                    _context.ScheduleSaturday
                        .Where(s => s.Instructor == instructor.FirstName + " " + instructor.LastName)
                        .ToList()
                        .OrderBy(s => s.Start)
                );


                foreach (var s in allSchedules)
                {
                    ws.Cell(row, 1).Value = $"{s.Start:hh\\:mm} - {s.End:hh\\:mm}";
                    ws.Cell(row, 2).Value = s.Day;
                    ws.Cell(row, 3).Value = s.Subject;
                    ws.Cell(row, 4).Value = s.Room;
                    row++;
                }

                ws.Columns().AdjustToContents();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var now = DateTime.Now;
            string formattedDate = now.ToString("yyyy_MMMM_dd", CultureInfo.InvariantCulture);
            string fileName = $"export_instructors_schedules_sti-alaminos_{formattedDate}.xlsx";

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

        public IActionResult OnPost()
        {
            Types = _context.Courses.Select(c => c.Name).ToList();

            if (string.IsNullOrWhiteSpace(NewInstructor.FirstName) ||
                string.IsNullOrWhiteSpace(NewInstructor.LastName) ||
                string.IsNullOrWhiteSpace(NewInstructor.Type))
            {
                Message = "All fields are required.";
                Instructors = _context.Instructors.ToList();
                return Page();
            }

            _context.Instructors.Add(NewInstructor);
            _context.SaveChanges();

            return RedirectToPage("/InstructorLoading");
        }

        public IActionResult OnPostDelete(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
