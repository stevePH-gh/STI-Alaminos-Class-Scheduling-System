using Class_Management.Data;
using Class_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Class_Management.Pages
{
    public class AccountsModel : PageModel
    {
        private readonly AppDbContext _context;

        public AccountsModel(AppDbContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; }

        public void OnGet()
        {
            Users = _context.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
