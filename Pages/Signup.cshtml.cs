using Class_Management.Data;
using Class_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace Class_Management.Pages
{
    public class SignupModel : PageModel
    {
        private readonly AppDbContext _context;

        public SignupModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User NewUser { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) // Caution esome empty fields
            {
                Message = "Please fill in all required fields.";
                return Page();
            }

            if (NewUser.Password != ConfirmPassword) // caution when password no match
            {
                Message = "Passwords do not match.";
                return Page();
            }

            bool exists = _context.Users.Any(u => u.Email == NewUser.Email);
            if (exists)
            {
                Message = "Email already registered.";
                return Page();
            }

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            Message = "Account created successfully!";
            return RedirectToPage("/Index");
        }
    }
}
