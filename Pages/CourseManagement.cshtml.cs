using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Class_Management.Data;
using Class_Management.Models;

namespace Class_Management.Pages
{
    public class CourseManagementModel : PageModel
    {
        private readonly AppDbContext _context;

        public CourseManagementModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course NewCourse { get; set; }

        public List<Course> Courses { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            Courses = _context.Courses.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Courses = _context.Courses.ToList();
                return Page();
            }

            _context.Courses.Add(NewCourse);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
    }
}
