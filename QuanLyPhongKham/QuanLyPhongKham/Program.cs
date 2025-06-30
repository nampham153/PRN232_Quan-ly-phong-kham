using BusinessAccessLayer.IService;
using BusinessAccessLayer.IService.Authen;
using BusinessAccessLayer.Service;
using BusinessAccessLayer.Service.Authen;
using DataAccessLayer.DAO;
using DataAccessLayer.DAO.Authen;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.IRepository.Authen;
using DataAccessLayer.models;
using DataAccessLayer.Repository;
using DataAccessLayer.Repository.Authen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;

namespace QuanLyPhongKham
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:7086") // Địa chỉ FE
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            // ========================================
            // Register DbContext
            // ========================================
            builder.Services.AddDbContext<ClinicDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"));
            });

            // ========================================
            // Register Services and Repositories
            // ========================================

            // Patient-related services
            builder.Services.AddScoped<MedicineDAO>();
            builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
            builder.Services.AddScoped<IMedicineService, MedicineService>();
            builder.Services.AddScoped<PatientDAO>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<ClinicDbContext>();
            builder.Services.AddScoped<PrescriptionDAO>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            builder.Services.AddScoped<ITestResultPdfService, TestResultPdfService>();



            // General Test Logic
            builder.Services.AddScoped<TestDAO>();
            builder.Services.AddScoped<ITestRepository, TestRepository>();
            builder.Services.AddScoped<ITestService, TestService>();

            builder.Services.AddScoped<ITestService, TestService>();
            builder.Services.AddScoped<ITestRepository, TestRepository>();
            builder.Services.AddScoped<DoctorDAO>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<MedicalRecordDAO>();
            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            builder.Services.AddScoped<DataAccessLayer.IRepository.IAccountRepository, DataAccessLayer.Repository.AccountRepository>();

            builder.Services.AddScoped<TestResultDAO>();
            builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();
            builder.Services.AddScoped<ITestResultService, TestResultService>();


            // ManagerUser Logic
            builder.Services.AddScoped<ManagerUserDAO>();
            builder.Services.AddScoped<IManagerUserRepository, ManagerUserRepository>();
            builder.Services.AddScoped<IManagerUserService, ManagerUserService>();

            // Account Logic
            builder.Services.AddScoped<AccountDAO>();
            builder.Services.AddScoped<DataAccessLayer.Repository.Authen.AccountRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddControllers()
                .AddOData(opt =>
                {
                    opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
                       .AddRouteComponents("odata", GetEdmModel());
                });


            // ========================================
            // Register MVC & JSON options
            // ========================================
            builder.Services.AddControllers();
            builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();


            // Đọc cấu hình JWT từ appsettings.json
            var jwtSecret = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"] ?? jwtIssuer; // Nếu không có Audience thì lấy Issuer

            // Đăng ký Authentication với JWT Bearer
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = jwtAudience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Cho phép token không cần prefix "Bearer "
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader))
                        {
                            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = authHeader.Substring("Bearer ".Length).Trim();
                            }
                            else
                            {
                                context.Token = authHeader.Trim();
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("admin"));   // hoặc "Admin"
                options.AddPolicy("Doctor", policy => policy.RequireRole("doctor")); // hoặc "Doctor"
                options.AddPolicy("Patient", policy => policy.RequireRole("patient"));// tương tự
            });

            // Cấu hình Swagger hỗ trợ Bearer token
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuanLyPhongKham API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Nhập token JWT của bạn, không cần tiền tố 'Bearer '",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDistributedMemoryCache(); // Cache cho Session

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            IEdmModel GetEdmModel()
            {
                var odataBuilder = new ODataConventionModelBuilder();
                odataBuilder.EntitySet<TestResult>("TestResults");
                odataBuilder.EntitySet<User>("Doctors");
                odataBuilder.EntitySet<MedicalRecord>("MedicalRecords");
                return odataBuilder.GetEdmModel();
            }

            // ========================================
            // Build & Configure Middleware
            // ========================================
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            //app.UseStaticFiles();

            //app.UseRouting();
            //app.UseSession(); // Phải nằm giữa UseRouting và UseAuthorization
            //app.UseAuthorization();
            //app.UseStaticFiles();

            //app.UseAuthentication();





            //app.MapControllers();
            //app.MapRazorPages();

            //app.Run();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //   app.UseStaticFiles(new StaticFileOptions
            //   {
            //       FileProvider = new PhysicalFileProvider(
            //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
            //       RequestPath = ""
            //   });
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();

            app.UseCors(); // Thêm dòng này trước UseAuthorization nếu có


            app.UseAuthentication();

            app.UseSession(); // Phải nằm giữa UseRouting và UseAuthorization

            app.UseAuthorization();
            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }

    }
}