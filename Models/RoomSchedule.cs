using System.ComponentModel.DataAnnotations;

namespace Class_Management.Models
{
    public class RoomSchedule
    {
        public int Id { get; set; }

        [Required]
        public string RoomName { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Instructor { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
