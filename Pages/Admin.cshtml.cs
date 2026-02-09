using Class_Management.Data;
using Class_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Class_Management.Pages
{
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _context;

        public AdminModel(AppDbContext context)
        {
            _context = context;
        }

        // Rooms for the schedule grid
        public List<Room> Rooms { get; set; }

        // Dropdown lists for schedule modal
        public List<Instructor> Instructors { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<Course> Courses { get; set; }

        // DAYS DATABASE
        public List<ScheduleMonday> MondaySchedules { get; set; }
        public List<ScheduleTuesday> TuesdaySchedules { get; set; }
        public List<ScheduleWednesday> WednesdaySchedules { get; set; }
        public List<ScheduleThursday> ThursdaySchedules { get; set; }
        public List<ScheduleFriday> FridaySchedules { get; set; }
        public List<ScheduleSaturday> SaturdaySchedules { get; set; }


        // =======================================================================
        // ON GET (initial page load)
        // =======================================================================
        public async Task OnGetAsync()
        {
            // Fetch Rooms
            Rooms = await _context.Rooms
                .OrderBy(r => !EF.Functions.Like(r.RoomName, "[0-9]%"))
                .ThenBy(r => r.RoomName.Length)
                .ThenBy(r => r.RoomName)
                .ToListAsync();

            // Fetch Instructors
            Instructors = await _context.Instructors
                .OrderBy(i => i.FirstName)
                .ThenBy(i => i.LastName)
                .ToListAsync();

            // Fetch Subjects
            Subjects = await _context.Subjects
                .OrderBy(s => s.Code)
                .ToListAsync();

            // Fetch Courses
            Courses = await _context.Courses
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Fetch ALL DAYS schedules (order by ticks)
            MondaySchedules = await _context.ScheduleMonday.OrderBy(s => s.StartTicks).ToListAsync();
            TuesdaySchedules = await _context.ScheduleTuesday.OrderBy(s => s.StartTicks).ToListAsync();
            WednesdaySchedules = await _context.ScheduleWednesday.OrderBy(s => s.StartTicks).ToListAsync();
            ThursdaySchedules = await _context.ScheduleThursday.OrderBy(s => s.StartTicks).ToListAsync();
            FridaySchedules = await _context.ScheduleFriday.OrderBy(s => s.StartTicks).ToListAsync();
            SaturdaySchedules = await _context.ScheduleSaturday.OrderBy(s => s.StartTicks).ToListAsync();

        }

        // =======================================================================
        // POST: Add a new schedule
        // =======================================================================
        public class ScheduleInputModel
        {
            public string Day { get; set; }
            public string Start { get; set; }   // e.g. "07:30"
            public string End { get; set; }     // e.g. "09:00"
            public string Room { get; set; }
            public string Instructor { get; set; }
            public string Subject { get; set; }
            public string Section { get; set; }
            public string Type { get; set; }
        }

        public async Task<IActionResult> OnPostAddScheduleAsync()
        {
            MondaySchedules = await _context.ScheduleMonday.ToListAsync();
            TuesdaySchedules = await _context.ScheduleTuesday.ToListAsync();
            WednesdaySchedules = await _context.ScheduleWednesday.ToListAsync();
            ThursdaySchedules = await _context.ScheduleThursday.ToListAsync();
            FridaySchedules = await _context.ScheduleFriday.ToListAsync();
            SaturdaySchedules = await _context.ScheduleSaturday.ToListAsync();

            var startTime = TimeSpan.Parse(Request.Form["Start"]);
            var endTime = TimeSpan.Parse(Request.Form["End"]);
            var room = Request.Form["Room"];
            var instructor = Request.Form["Instructor"];
            var subject = Request.Form["Subject"];
            var sectionName = Request.Form["SectionName"];
            var sectionID = Request.Form["SectionID"];
            var type = Request.Form["Type"];
            var day = Request.Form["Day"];

            // RULE: Only 1 Lecture and 1 Laboratory per subject per week
            var weeklyCheck = CheckWeeklyLimit(subject, type, $"{sectionName} {sectionID}".Trim());

            if (weeklyCheck.Exists)
            {
                TempData["ScheduleConflict"] =
                    $"{type} for this subject already exists: {weeklyCheck.Day}, {weeklyCheck.Start}-{weeklyCheck.End} at {weeklyCheck.Room}";
                await OnGetAsync();
                return RedirectToPage();
            }

            // Combine section name and ID
            var fullSection = $"{sectionName} {sectionID}".Trim();

            // -------------------- SECTION CONFLICT CHECK --------------------
            dynamic sectionConflict = null;

            if (day == "MONDAY")
            {
                sectionConflict = MondaySchedules.FirstOrDefault(s =>
                    startTime.Ticks < s.EndTicks && endTime.Ticks > s.StartTicks &&
                    s.Section == fullSection);
            }
            else if (day == "TUESDAY")
            {
                sectionConflict = TuesdaySchedules.FirstOrDefault(s =>
                    startTime.Ticks < s.EndTicks && endTime.Ticks > s.StartTicks &&
                    s.Section == fullSection);
            }
            else if (day == "WEDNESDAY")
            {
                sectionConflict = WednesdaySchedules.FirstOrDefault(s =>
                    startTime.Ticks < s.EndTicks && endTime.Ticks > s.StartTicks &&
                    s.Section == fullSection);
            }
            else if (day == "THURSDAY")
            {
                sectionConflict = ThursdaySchedules.FirstOrDefault(s =>
                    startTime.Ticks < s.EndTicks && endTime.Ticks > s.StartTicks &&
                    s.Section == fullSection);
            }
            else if (day == "FRIDAY")
            {
                sectionConflict = FridaySchedules.FirstOrDefault(s =>
                    startTime.Ticks < s.EndTicks && endTime.Ticks > s.StartTicks &&
                    s.Section == fullSection);
            }
            else if (day == "SATURDAY")
            {
                sectionConflict = SaturdaySchedules.FirstOrDefault(s =>
                    startTime.Ticks < s.EndTicks && endTime.Ticks > s.StartTicks &&
                    s.Section == fullSection);
            }

            // If conflict exists, show message and redirect
            if (sectionConflict != null)
            {
                TempData["ScheduleConflict"] =
                    $"Section {fullSection} already has a schedule from {sectionConflict.Start:hh\\:mm} to {sectionConflict.End:hh\\:mm} in {sectionConflict.Room}!";
                await OnGetAsync();
                return RedirectToPage();
            }


            var conflictBlock = GetConflict(startTime, endTime, room, instructor, day);
            if (conflictBlock != null)
            {
                TempData["ScheduleConflict"] = $"Schedule conflict with {conflictBlock.Instructor} from {conflictBlock.Start:hh\\:mm} to {conflictBlock.End:hh\\:mm}!";
                await OnGetAsync();
                return RedirectToPage();
            }

            switch (day)
            {
                case "MONDAY":
                    _context.ScheduleMonday.Add(new ScheduleMonday
                    {
                        StartTicks = startTime.Ticks,
                        EndTicks = endTime.Ticks,
                        Room = room,
                        Instructor = instructor,
                        Subject = subject,
                        Section = $"{sectionName} {sectionID}".Trim(),
                        Type = type
                    });

                    break;

                case "TUESDAY":
                    _context.ScheduleTuesday.Add(new ScheduleTuesday
                    {
                        StartTicks = startTime.Ticks,
                        EndTicks = endTime.Ticks,
                        Room = room,
                        Instructor = instructor,
                        Subject = subject,
                        Section = $"{sectionName} {sectionID}".Trim(),
                        Type = type
                    });

                    break;

                case "WEDNESDAY":
                    _context.ScheduleWednesday.Add(new ScheduleWednesday
                    {
                        StartTicks = startTime.Ticks,
                        EndTicks = endTime.Ticks,
                        Room = room,
                        Instructor = instructor,
                        Subject = subject,
                        Section = $"{sectionName} {sectionID}".Trim(),
                        Type = type
                    });

                    break;

                case "THURSDAY":
                    _context.ScheduleThursday.Add(new ScheduleThursday
                    {
                        StartTicks = startTime.Ticks,
                        EndTicks = endTime.Ticks,
                        Room = room,
                        Instructor = instructor,
                        Subject = subject,
                        Section = $"{sectionName} {sectionID}".Trim(),
                        Type = type
                    });

                    break;

                case "FRIDAY":
                    _context.ScheduleFriday.Add(new ScheduleFriday
                    {
                        StartTicks = startTime.Ticks,
                        EndTicks = endTime.Ticks,
                        Room = room,
                        Instructor = instructor,
                        Subject = subject,
                        Section = $"{sectionName} {sectionID}".Trim(),
                        Type = type
                    });

                    break;

                case "SATURDAY":
                    _context.ScheduleSaturday.Add(new ScheduleSaturday
                    {
                        StartTicks = startTime.Ticks,
                        EndTicks = endTime.Ticks,
                        Room = room,
                        Instructor = instructor,
                        Subject = subject,
                        Section = $"{sectionName} {sectionID}".Trim(),
                        Type = type
                    });

                    break;
            }

            await _context.SaveChangesAsync();

            // Set success message
            TempData["ScheduleSuccess"] = $"Schedule successfully added for {instructor} from {startTime:hh\\:mm} to {endTime:hh\\:mm}!";

            return RedirectToPage();


        }

        // -------------------------------------------------------------------
        // Returns conflicting schedule block or null
        // -------------------------------------------------------------------
        private dynamic GetConflict(TimeSpan start, TimeSpan end, string room, string instructor, string day)
        {
            var startTicks = start.Ticks;
            var endTicks = end.Ticks;
            switch (day)
            {
                case "MONDAY":
                    return _context.ScheduleMonday.FirstOrDefault(s =>
                        startTicks < s.EndTicks && endTicks > s.StartTicks &&
                        (s.Room == room || s.Instructor == instructor));

                case "TUESDAY":
                    return _context.ScheduleTuesday.FirstOrDefault(s =>
                        startTicks < s.EndTicks && endTicks > s.StartTicks &&
                        (s.Room == room || s.Instructor == instructor));

                case "WEDNESDAY":
                    return _context.ScheduleWednesday.FirstOrDefault(s =>
                        startTicks < s.EndTicks && endTicks > s.StartTicks &&
                        (s.Room == room || s.Instructor == instructor));

                case "THURSDAY":
                    return _context.ScheduleThursday.FirstOrDefault(s =>
                        startTicks < s.EndTicks && endTicks > s.StartTicks &&
                        (s.Room == room || s.Instructor == instructor));

                case "FRIDAY":
                    return _context.ScheduleFriday.FirstOrDefault(s =>
                        startTicks < s.EndTicks && endTicks > s.StartTicks &&
                        (s.Room == room || s.Instructor == instructor));

                case "SATURDAY":
                    return _context.ScheduleSaturday.FirstOrDefault(s =>
                        startTicks < s.EndTicks && endTicks > s.StartTicks &&
                        (s.Room == room || s.Instructor == instructor));

                default:
                    return null;
            }
        }

        // -------------------------------------------------------------------
        // 1 LAB AND 1 LEC DETECTION
        // -------------------------------------------------------------------

        private (bool Exists, string Day, string Start, string End, string Room)
        CheckWeeklyLimit(string subjectName, string type, string section)
        {
            var week = new List<(string Day, dynamic List)>
            {
                ("MONDAY", MondaySchedules),
                ("TUESDAY", TuesdaySchedules),
                ("WEDNESDAY", WednesdaySchedules),
                ("THURSDAY", ThursdaySchedules),
                ("FRIDAY", FridaySchedules),
                ("SATURDAY", SaturdaySchedules)
            };

            foreach (var d in week)
            {
                if (d.List == null) continue;

                foreach (var s in d.List)
                {
                    if (s.Subject == subjectName &&
                        s.Type == type &&
                        s.Section == section)
                        {
                        return (true,
                        d.Day,
                        s.Start.ToString(@"hh\:mm"),
                        s.End.ToString(@"hh\:mm"),
                        s.Room);
                        }
                    }
                }
            return (false, null, null, null, null);
        }

    }
}