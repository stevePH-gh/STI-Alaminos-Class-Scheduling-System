using Class_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Class_Management.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<RoomSchedule> RoomSchedules { get; set; } // ROOM SCHEDULES // IGNORE DIS

        public DbSet<User> Users { get; set; } // ACCOUNT DATABASE // ilagay ko lng incase
        public DbSet<Instructor> Instructors { get; set; } // Instructors database
        public DbSet<Subject> Subjects { get; set; } // Subjects database
        public DbSet<Room> Rooms { get; set; } // Rooms database
        public DbSet<Course> Courses { get; set; } // Courses database

        // DAYS DATABASE:
        public DbSet<ScheduleMonday> ScheduleMonday { get; set; }
        public DbSet<ScheduleTuesday> ScheduleTuesday { get; set; }
        public DbSet<ScheduleWednesday> ScheduleWednesday { get; set; }
        public DbSet<ScheduleThursday> ScheduleThursday { get; set; }
        public DbSet<ScheduleFriday> ScheduleFriday { get; set; }
        public DbSet<ScheduleSaturday> ScheduleSaturday { get; set; }

    }
}
