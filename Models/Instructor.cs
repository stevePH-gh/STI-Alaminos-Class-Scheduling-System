using System.ComponentModel.DataAnnotations;

namespace Class_Management.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Type { get; set; }

        public bool? MinorSub { get; set; }
        public bool? MathSub { get; set; }

    }
}
