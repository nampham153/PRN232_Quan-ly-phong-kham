using BusinessAccessLayer.IService;
using BusinessAccessLayer.Service;
using DataAccessLayer.DAO;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.Repository;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;

namespace QuanLyPhongKham
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DAO / Repository / Service DI
            builder.Services.AddScoped<MedicineDAO>();
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<IMedicineService, MedicineService>();

            builder.Services.AddScoped<TestDAO>();
            builder.Services.AddScoped<ITestService, TestService>();
            builder.Services.AddScoped<ITestRepository, TestRepository>();

            builder.Services.AddScoped<TestResultDAO>();
            builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();
            builder.Services.AddScoped<ITestResultService, TestResultService>();

            // OData + Controllers + Swagger
            builder.Services.AddControllers()
                .AddOData(opt =>
                {
                    opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
                       .AddRouteComponents("odata", GetEdmModel());
                });

            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ClinicDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"))
            );

            IEdmModel GetEdmModel()
            {
                var odataBuilder = new ODataConventionModelBuilder();
                odataBuilder.EntitySet<TestResult>("TestResults");
                return odataBuilder.GetEdmModel();
            }

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