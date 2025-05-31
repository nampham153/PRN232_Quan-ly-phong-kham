using BusinessAccessLayer.IService;
using BusinessAccessLayer.Service;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace QuanLyPhongKham
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ClinicDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

            // Changed to Scoped instead of Singleton
            builder.Services.AddScoped<ITestService, TestService>();
            builder.Services.AddScoped<ITestRepository, TestRepository>();

            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}