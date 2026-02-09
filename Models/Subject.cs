using System.ComponentModel.DataAnnotations;

namespace Class_Management.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        public string SubjectName { get; set; }

        [Required]
        public string Type { get; set; }  // e.g., "ITP", "HRS/HRA"

        [Required]
        public string Code { get; set; }
    }
}
