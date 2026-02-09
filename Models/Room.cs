using System.ComponentModel.DataAnnotations;

namespace Class_Management.Models
{
    public class Room
    {
        public int Id { get; set; } // Primary key

        [Required]
        public string RoomName { get; set; }

        [Required]
        public string Type { get; set; } // "Lab" or "Regular"
    }
}
