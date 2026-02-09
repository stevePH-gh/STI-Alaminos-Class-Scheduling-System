using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Class_Management.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }


        [Required]
        public string Sex { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? Role { get; set; } = "";

    }
}
