using Class_Management.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // Required for HttpContext.Session
using System.Linq;

namespace Class_Management.Pages
{
    public class loginModel : PageModel
    {
        private readonly AppDbContext _context;

        public loginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                Message = "Account not found.";
                return Page();
            }

            if (user.Password != Password)
            {
                Message = "Incorrect password.";
                return Page();
            }

            // Save login info to session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role ?? "std"); // If Role is null, default to "std" (student)

            // Redirect to Index after successful login
            return RedirectToPage("/Index");
        }
    }
}
