using Class_Management.Data;
using Microsoft.EntityFrameworkCore;

namespace Class_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. eto ung mag enable to all pages
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("LiteConnection")));


            // Enable Session support, session para ma detect ung roles
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // optional: auto-logout after 30 minutes
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add Session BEFORE Authorization
            app.UseSession();

            app.UseAuthorization();

            app.MapRazorPages();

            /*
                                                                 // UNCOMMENT THIS UPON DEPLOYMENT
            // Automatically open browser when app starts
            app.Urls.Add("http://localhost:5000"); // Optional, default is 5000

            var lifetime = app.Lifetime;
            
            lifetime.ApplicationStarted.Register(() =>
            {
                var url = "http://localhost:5000";
                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    System.Diagnostics.Process.Start(psi);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to open browser: " + ex.Message);
                }
            }); */
            
            app.Run();

        }
    }
}
