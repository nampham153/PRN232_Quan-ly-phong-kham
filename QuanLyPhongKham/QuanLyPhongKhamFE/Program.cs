using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using System.Text;

namespace QuanLyPhongKham.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient(); // Để call API từ Backend

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            // Default route
            app.MapGet("/", () => Results.Redirect("/TestPage"));

            Console.WriteLine("=== Frontend - Clinic Management System ===");
            Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
            Console.WriteLine("Frontend running on: https://localhost:7086 (or check console)");
            Console.WriteLine("Backend API: https://localhost:7086");

            app.Run();
        }
    }
}