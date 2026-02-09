using Class_Management.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;

namespace Class_Management.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string RecoverEmail { get; set; }
        public string RecoverMessage { get; set; }

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
            // HttpContext.Session.SetString("UserRole", user.Role ?? "std"); // If Role is null, default to "std" (student)

            // Redirect to admin panel after successful login
            return RedirectToPage("/Overview");
        }

        public IActionResult OnPostRecover()
        {
            if (string.IsNullOrEmpty(RecoverEmail))
            {
                TempData["RecoverMessage"] = "Please enter your email.";
                return RedirectToPage();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == RecoverEmail);

            if (user == null)
            {
                TempData["RecoverError"] = "Email not found in the system.";
            }
            else
            {
                TempData["RecoverSuccess"] = "Email has been sent!";

                // SEND TEST EMAIL HERE
                SendTestEmail(RecoverEmail);
            }

            return RedirectToPage();
        }

        /* ============================ DO NOT ERASE !!! =========================== */
        /* 
         * EMAIL INFO FOR FORGOT EMAIL FEATURE 
         * 
         * Email:       alaminosschedulingsystem@gmail.com
         * 
         * Passw:       alaminosclassschedulingSTIalaminos
         * 
         * Birth:       August 21, 1983 (Date where STI College was Founded)
         * 
         * First Name:  STI
         * Last Name:   Alaminos
         * 
         * Access Code: kmzg gxsu znqv ygww
         */
        /* ========================================================================= */

        public void SendTestEmail(string toEmail)
        {
            try
            {
                
                var user = _context.Users.FirstOrDefault(u => u.Email == toEmail);
                if (user == null)
                {
                    Console.WriteLine("Email not found in the database.");
                    return;
                }

                var fromEmail = "alaminosschedulingsystem@gmail.com";
                var fromPassword = "kmzg gxsu znqv ygww";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

var body = $@"
Hello {user.FirstName} {user.LastName},

We received a password recovery request for your account. Here are your details:

- Email: {user.Email}
- Birthday: {user.Birthday.ToShortDateString()}
- Sex: {user.Sex}
- Your Password: {user.Password}

STI Alaminos Scheduling System
                            ";

                // Create the email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, "STI Alaminos Scheduling System"),
                    Subject = "Password Recovery",
                    Body = body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(toEmail);

                // Send email
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }

    }
}