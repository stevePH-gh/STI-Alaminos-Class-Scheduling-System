using System.ComponentModel.DataAnnotations.Schema;

namespace Class_Management.Models
{
    public class ScheduleMonday
    {
        public int Id { get; set; }
        public string Day { get; set; } = "Monday";

        //  WAG GALAWIN
        public long StartTicks { get; set; }

        [NotMapped]
        public TimeSpan Start
        {
            get => TimeSpan.FromTicks(StartTicks);
            set => StartTicks = value.Ticks;
        }

        public long EndTicks { get; set; }

        [NotMapped]
        public TimeSpan End
        {
            get => TimeSpan.FromTicks(EndTicks);
            set => EndTicks = value.Ticks;
        }

        public string Room { get; set; }
        public string Instructor { get; set; }
        public string Subject { get; set; }
        public string Section { get; set; }
        public string Type { get; set; }
    }
}
