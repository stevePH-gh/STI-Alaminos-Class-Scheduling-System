using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Class_Management.Models; // <-- your Subject & Course classes
using Class_Management.Data;   // <-- your DbContext
using System.Collections.Generic;
using System.Linq;

namespace Class_Management.Pages
{
    public class SubjectManagementModel : PageModel
    {
        private readonly AppDbContext _context;

        public SubjectManagementModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Subject NewSubject { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<Course> Courses { get; set; } // <-- new

        public string Message { get; set; }

        public void OnGet()
        {
            Subjects = _context.Subjects.ToList();
            Courses = _context.Courses.ToList(); // <-- fetch courses dynamically
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Subjects = _context.Subjects.ToList();
                Courses = _context.Courses.ToList(); // <-- make sure courses are loaded on validation failure
                return Page();
            }

            _context.Subjects.Add(NewSubject);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
