using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.dbcontext
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext()
        {
        }

        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithOne(u => u.Account)
                .HasForeignKey<User>(u => u.AccountId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Patient)
                .WithOne(p => p.Account)
                .HasForeignKey<Patient>(p => p.AccountId);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.RefreshToken)
                .WithOne(r => r.Account)
                .HasForeignKey<RefreshToken>(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TestResult>()
                .HasOne(t => t.User)
                .WithMany(u => u.TestResults)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles (4 roles)
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Doctor" },
                new Role { RoleId = 3, RoleName = "Staff" },
                new Role { RoleId = 4, RoleName = "Patient" }
            );

            // Seed Accounts (20 accounts - 1 admin, 10 doctors, 1 staff, 8 patients)
            modelBuilder.Entity<Account>().HasData(
                new Account { AccountId = 1, Username = "admin1", PasswordHash = "hashed_password_1", RoleId = 1, Status = true },
                new Account { AccountId = 2, Username = "doctor1", PasswordHash = "hashed_password_2", RoleId = 2, Status = true },
                new Account { AccountId = 3, Username = "doctor2", PasswordHash = "hashed_password_3", RoleId = 2, Status = true },
                new Account { AccountId = 4, Username = "doctor3", PasswordHash = "hashed_password_4", RoleId = 2, Status = true },
                new Account { AccountId = 5, Username = "doctor4", PasswordHash = "hashed_password_5", RoleId = 2, Status = true },
                new Account { AccountId = 6, Username = "doctor5", PasswordHash = "hashed_password_6", RoleId = 2, Status = true },
                new Account { AccountId = 7, Username = "doctor6", PasswordHash = "hashed_password_7", RoleId = 2, Status = true },
                new Account { AccountId = 8, Username = "doctor7", PasswordHash = "hashed_password_8", RoleId = 2, Status = true },
                new Account { AccountId = 9, Username = "doctor8", PasswordHash = "hashed_password_9", RoleId = 2, Status = true },
                new Account { AccountId = 10, Username = "doctor9", PasswordHash = "hashed_password_10", RoleId = 2, Status = true },
                new Account { AccountId = 11, Username = "doctor10", PasswordHash = "hashed_password_11", RoleId = 2, Status = true }, // Thêm bác sĩ thứ 10
                new Account { AccountId = 12, Username = "staff1", PasswordHash = "hashed_password_12", RoleId = 3, Status = true },
                new Account { AccountId = 13, Username = "patient1", PasswordHash = "hashed_password_13", RoleId = 4, Status = true },
                new Account { AccountId = 14, Username = "patient2", PasswordHash = "hashed_password_14", RoleId = 4, Status = true },
                new Account { AccountId = 15, Username = "patient3", PasswordHash = "hashed_password_15", RoleId = 4, Status = true },
                new Account { AccountId = 16, Username = "patient4", PasswordHash = "hashed_password_16", RoleId = 4, Status = true },
                new Account { AccountId = 17, Username = "patient5", PasswordHash = "hashed_password_17", RoleId = 4, Status = true },
                new Account { AccountId = 18, Username = "patient6", PasswordHash = "hashed_password_18", RoleId = 4, Status = true },
                new Account { AccountId = 19, Username = "patient7", PasswordHash = "hashed_password_19", RoleId = 4, Status = true },
                new Account { AccountId = 20, Username = "patient8", PasswordHash = "hashed_password_20", RoleId = 4, Status = true }
            );

            // Seed Users (11 users - 1 admin, 10 doctors, 1 staff)
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FullName = "Admin User", Gender = "Male", DOB = new DateTime(1975, 1, 1), Phone = "0901234560", Email = "admin@clinic.com", AccountId = 1 },
                new User { UserId = 2, FullName = "Dr. Nguyen Van A", Gender = "Male", DOB = new DateTime(1980, 5, 15), Phone = "0901234567", Email = "doctor1@clinic.com", AccountId = 2 },
                new User { UserId = 3, FullName = "Dr. Le Thi B", Gender = "Female", DOB = new DateTime(1985, 8, 20), Phone = "0901234568", Email = "doctor2@clinic.com", AccountId = 3 },
                new User { UserId = 4, FullName = "Dr. Tran Van C", Gender = "Male", DOB = new DateTime(1978, 12, 25), Phone = "0901234570", Email = "doctor3@clinic.com", AccountId = 4 },
                new User { UserId = 5, FullName = "Dr. Pham Minh D", Gender = "Male", DOB = new DateTime(1982, 7, 18), Phone = "0901234571", Email = "doctor4@clinic.com", AccountId = 5 },
                new User { UserId = 6, FullName = "Dr. Hoang Thi E", Gender = "Female", DOB = new DateTime(1975, 11, 5), Phone = "0901234572", Email = "doctor5@clinic.com", AccountId = 6 },
                new User { UserId = 7, FullName = "Dr. Vu Van F", Gender = "Male", DOB = new DateTime(1988, 4, 22), Phone = "0901234573", Email = "doctor6@clinic.com", AccountId = 7 },
                new User { UserId = 8, FullName = "Dr. Dang Thi G", Gender = "Female", DOB = new DateTime(1983, 9, 14), Phone = "0901234574", Email = "doctor7@clinic.com", AccountId = 8 },
                new User { UserId = 9, FullName = "Dr. Bui Van H", Gender = "Male", DOB = new DateTime(1987, 6, 30), Phone = "0901234575", Email = "doctor8@clinic.com", AccountId = 9 },
                new User { UserId = 10, FullName = "Dr. Do Thi I", Gender = "Female", DOB = new DateTime(1981, 2, 8), Phone = "0901234576", Email = "doctor9@clinic.com", AccountId = 10 },
                new User { UserId = 11, FullName = "Dr. Ngo Van J", Gender = "Male", DOB = new DateTime(1984, 3, 12), Phone = "0901234577", Email = "doctor10@clinic.com", AccountId = 11 },
                new User { UserId = 12, FullName = "Staff Ngo Van K", Gender = "Male", DOB = new DateTime(1990, 3, 10), Phone = "0901234569", Email = "staff1@clinic.com", AccountId = 12 }
            );

            // Seed Patients (8 patients)
            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, FullName = "Nguyen Thi K", Gender = "Female", DOB = new DateTime(1995, 3, 15), Phone = "0987654321", Email = "patient1@email.com", Address = "123 Nguyen Trai, Hanoi", AccountId = 13, AvatarPath = "/images/avatars/patient1.jpg", UnderlyingDiseases = "Hypertension", DiseaseDetails = "Diagnosed with hypertension in 2020, managed with lifestyle changes and medication." },
                new Patient { PatientId = 2, FullName = "Tran Van L", Gender = "Male", DOB = new DateTime(1992, 8, 20), Phone = "0987654322", Email = "patient2@email.com", Address = "456 Le Loi, Ho Chi Minh", AccountId = 14, AvatarPath = "/images/avatars/patient2.jpg", UnderlyingDiseases = "None", DiseaseDetails = "No significant chronic conditions, occasional muscle strain from physical activity." },
                new Patient { PatientId = 3, FullName = "Le Thi M", Gender = "Female", DOB = new DateTime(1988, 12, 10), Phone = "0987654323", Email = "patient3@email.com", Address = "789 Tran Phu, Da Nang", AccountId = 15, AvatarPath = "/images/avatars/patient3.jpg", UnderlyingDiseases = "Asthma", DiseaseDetails = "Diagnosed with asthma at age 15, uses inhaler as needed." },
                new Patient { PatientId = 4, FullName = "Hoang Van N", Gender = "Male", DOB = new DateTime(1990, 5, 25), Phone = "0987654324", Email = "patient4@email.com", Address = "321 Hai Ba Trung, Hue", AccountId = 16, AvatarPath = "/images/avatars/patient4.jpg", UnderlyingDiseases = "Hypertension, Type 2 Diabetes", DiseaseDetails = "Hypertension since 2018, Type 2 Diabetes diagnosed in 2021, on metformin." },
                new Patient { PatientId = 5, FullName = "Pham Thi O", Gender = "Female", DOB = new DateTime(1985, 7, 18), Phone = "0987654325", Email = "patient5@email.com", Address = "654 Dong Khoi, Can Tho", AccountId = 17, AvatarPath = "/images/avatars/patient5.jpg", UnderlyingDiseases = "Gastritis", DiseaseDetails = "Chronic gastritis diagnosed in 2019, managed with PPI and diet." },
                new Patient { PatientId = 6, FullName = "Vu Van P", Gender = "Male", DOB = new DateTime(1993, 11, 5), Phone = "0987654326", Email = "patient6@email.com", Address = "987 Bach Dang, Hai Phong", AccountId = 18, AvatarPath = "/images/avatars/patient6.jpg", UnderlyingDiseases = "Arthritis", DiseaseDetails = "Rheumatoid arthritis diagnosed in 2022, on anti-inflammatory medication." },
                new Patient { PatientId = 7, FullName = "Dang Thi Q", Gender = "Female", DOB = new DateTime(1987, 4, 22), Phone = "0987654327", Email = "patient7@email.com", Address = "147 Ly Thuong Kiet, Nha Trang", AccountId = 19, AvatarPath = "/images/avatars/patient7.jpg", UnderlyingDiseases = "Allergic Rhinitis", DiseaseDetails = "Seasonal allergies since childhood, managed with antihistamines." },
                new Patient { PatientId = 8, FullName = "Bui Van R", Gender = "Male", DOB = new DateTime(1991, 9, 14), Phone = "0987654328", Email = "patient8@email.com", Address = "258 Quang Trung, Vung Tau", AccountId = 20, AvatarPath = "/images/avatars/patient8.jpg", UnderlyingDiseases = "Type 2 Diabetes", DiseaseDetails = "Diagnosed with Type 2 Diabetes in 2020, controlled with metformin and diet." }
            );

            // Seed Tests (10 tests)
            modelBuilder.Entity<Test>().HasData(
                new Test { TestId = 1, TestName = "Blood Test", Description = "Complete blood count analysis" },
                new Test { TestId = 2, TestName = "X-Ray", Description = "Chest X-ray examination" },
                new Test { TestId = 3, TestName = "ECG", Description = "Electrocardiogram test" },
                new Test { TestId = 4, TestName = "Ultrasound", Description = "Abdominal ultrasound" },
                new Test { TestId = 5, TestName = "CT Scan", Description = "Computed tomography scan" },
                new Test { TestId = 6, TestName = "MRI", Description = "Magnetic resonance imaging" },
                new Test { TestId = 7, TestName = "Urine Test", Description = "Urinalysis examination" },
                new Test { TestId = 8, TestName = "Blood Sugar", Description = "Glucose level test" },
                new Test { TestId = 9, TestName = "Cholesterol", Description = "Lipid profile test" },
                new Test { TestId = 10, TestName = "Blood Pressure", Description = "Hypertension screening" }
            );

            // Seed Medicines (10 medicines)
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { MedicineId = 1, MedicineName = "Paracetamol", Unit = "tablet", Usage = "Take 1-2 tablets every 4-6 hours" },
                new Medicine { MedicineId = 2, MedicineName = "Amoxicillin", Unit = "capsule", Usage = "Take 1 capsule 3 times daily" },
                new Medicine { MedicineId = 3, MedicineName = "Ibuprofen", Unit = "tablet", Usage = "Take 1 tablet every 8 hours" },
                new Medicine { MedicineId = 4, MedicineName = "Aspirin", Unit = "tablet", Usage = "Take 1 tablet daily" },
                new Medicine { MedicineId = 5, MedicineName = "Metformin", Unit = "tablet", Usage = "Take 1 tablet twice daily with meals" },
                new Medicine { MedicineId = 6, MedicineName = "Lisinopril", Unit = "tablet", Usage = "Take 1 tablet once daily" },
                new Medicine { MedicineId = 7, MedicineName = "Omeprazole", Unit = "capsule", Usage = "Take 1 capsule before breakfast" },
                new Medicine { MedicineId = 8, MedicineName = "Salbutamol", Unit = "inhaler", Usage = "2 puffs when needed" },
                new Medicine { MedicineId = 9, MedicineName = "Diazepam", Unit = "tablet", Usage = "Take 1 tablet when needed" },
                new Medicine { MedicineId = 10, MedicineName = "Cetirizine", Unit = "tablet", Usage = "Take 1 tablet once daily" }
            );

            // Seed Medical Records (8 records) - Đảm bảo khớp với số lượng Patient
            modelBuilder.Entity<MedicalRecord>().HasData(
                new MedicalRecord { RecordId = 1, PatientId = 1, UserId = 2, Date = DateTime.Now.AddDays(-30), Symptoms = "Fever, headache", Diagnosis = "Common cold", Note = "Rest and fluids recommended" },
                new MedicalRecord { RecordId = 2, PatientId = 2, UserId = 3, Date = DateTime.Now.AddDays(-25), Symptoms = "Chest pain", Diagnosis = "Muscle strain", Note = "Apply heat therapy" },
                new MedicalRecord { RecordId = 3, PatientId = 3, UserId = 4, Date = DateTime.Now.AddDays(-20), Symptoms = "Shortness of breath", Diagnosis = "Asthma", Note = "Prescribed inhaler" },
                new MedicalRecord { RecordId = 4, PatientId = 4, UserId = 5, Date = DateTime.Now.AddDays(-15), Symptoms = "High blood pressure", Diagnosis = "Hypertension", Note = "Lifestyle changes needed" },
                new MedicalRecord { RecordId = 5, PatientId = 5, UserId = 6, Date = DateTime.Now.AddDays(-10), Symptoms = "Stomach pain", Diagnosis = "Gastritis", Note = "Avoid spicy foods" },
                new MedicalRecord { RecordId = 6, PatientId = 6, UserId = 7, Date = DateTime.Now.AddDays(-8), Symptoms = "Joint pain", Diagnosis = "Arthritis", Note = "Physical therapy recommended" },
                new MedicalRecord { RecordId = 7, PatientId = 7, UserId = 8, Date = DateTime.Now.AddDays(-5), Symptoms = "Skin rash", Diagnosis = "Allergic reaction", Note = "Avoid allergens" },
                new MedicalRecord { RecordId = 8, PatientId = 8, UserId = 9, Date = DateTime.Now.AddDays(-3), Symptoms = "Diabetes symptoms", Diagnosis = "Type 2 Diabetes", Note = "Diet control important" }
            );

            // Seed Test Results (8 results) - Đảm bảo khớp với số lượng MedicalRecord
            modelBuilder.Entity<TestResult>().HasData(
                new TestResult { ResultId = 1, RecordId = 1, TestId = 1, UserId = 2, ResultDetail = "WBC: 8.5, RBC: 4.2", TestDate = DateTime.Now.AddDays(-29) },
                new TestResult { ResultId = 2, RecordId = 2, TestId = 2, UserId = 3, ResultDetail = "Chest clear, no abnormalities", TestDate = DateTime.Now.AddDays(-24) },
                new TestResult { ResultId = 3, RecordId = 3, TestId = 3, UserId = 4, ResultDetail = "Normal heart rhythm", TestDate = DateTime.Now.AddDays(-19) },
                new TestResult { ResultId = 4, RecordId = 4, TestId = 10, UserId = 5, ResultDetail = "BP: 140/90 mmHg", TestDate = DateTime.Now.AddDays(-14) },
                new TestResult { ResultId = 5, RecordId = 5, TestId = 4, UserId = 6, ResultDetail = "Mild gastric inflammation", TestDate = DateTime.Now.AddDays(-9) },
                new TestResult { ResultId = 6, RecordId = 6, TestId = 2, UserId = 7, ResultDetail = "Joint space narrowing", TestDate = DateTime.Now.AddDays(-7) },
                new TestResult { ResultId = 7, RecordId = 7, TestId = 1, UserId = 8, ResultDetail = "Elevated eosinophils", TestDate = DateTime.Now.AddDays(-4) },
                new TestResult { ResultId = 8, RecordId = 8, TestId = 8, UserId = 9, ResultDetail = "Glucose: 180 mg/dL", TestDate = DateTime.Now.AddDays(-2) }
            );

            // Seed Prescriptions (8 prescriptions) - Đảm bảo khớp với số lượng MedicalRecord
            modelBuilder.Entity<Prescription>().HasData(
                new Prescription { PrescriptionId = 1, RecordId = 1, MedicineId = 1, Dosage = "500mg twice daily", Date = DateTime.Now.AddDays(-30) },
                new Prescription { PrescriptionId = 2, RecordId = 2, MedicineId = 3, Dosage = "400mg three times daily", Date = DateTime.Now.AddDays(-25) },
                new Prescription { PrescriptionId = 3, RecordId = 3, MedicineId = 8, Dosage = "2 puffs as needed", Date = DateTime.Now.AddDays(-20) },
                new Prescription { PrescriptionId = 4, RecordId = 4, MedicineId = 6, Dosage = "10mg once daily", Date = DateTime.Now.AddDays(-15) },
                new Prescription { PrescriptionId = 5, RecordId = 5, MedicineId = 7, Dosage = "20mg before breakfast", Date = DateTime.Now.AddDays(-10) },
                new Prescription { PrescriptionId = 6, RecordId = 6, MedicineId = 3, Dosage = "200mg twice daily", Date = DateTime.Now.AddDays(-8) },
                new Prescription { PrescriptionId = 7, RecordId = 7, MedicineId = 10, Dosage = "10mg once daily", Date = DateTime.Now.AddDays(-5) },
                new Prescription { PrescriptionId = 8, RecordId = 8, MedicineId = 5, Dosage = "500mg twice daily", Date = DateTime.Now.AddDays(-3) }
            );

            // Seed Refresh Tokens (20 tokens - one for each account)
            modelBuilder.Entity<RefreshToken>().HasData(
                new RefreshToken { TokenId = 1, Token = "refresh_token_1", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 1 },
                new RefreshToken { TokenId = 2, Token = "refresh_token_2", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 2 },
                new RefreshToken { TokenId = 3, Token = "refresh_token_3", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 3 },
                new RefreshToken { TokenId = 4, Token = "refresh_token_4", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 4 },
                new RefreshToken { TokenId = 5, Token = "refresh_token_5", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 5 },
                new RefreshToken { TokenId = 6, Token = "refresh_token_6", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 6 },
                new RefreshToken { TokenId = 7, Token = "refresh_token_7", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 7 },
                new RefreshToken { TokenId = 8, Token = "refresh_token_8", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 8 },
                new RefreshToken { TokenId = 9, Token = "refresh_token_9", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 9 },
                new RefreshToken { TokenId = 10, Token = "refresh_token_10", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 10 },
                new RefreshToken { TokenId = 11, Token = "refresh_token_11", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 11 },
                new RefreshToken { TokenId = 12, Token = "refresh_token_12", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 12 },
                new RefreshToken { TokenId = 13, Token = "refresh_token_13", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 13 },
                new RefreshToken { TokenId = 14, Token = "refresh_token_14", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 14 },
                new RefreshToken { TokenId = 15, Token = "refresh_token_15", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 15 },
                new RefreshToken { TokenId = 16, Token = "refresh_token_16", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 16 },
                new RefreshToken { TokenId = 17, Token = "refresh_token_17", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 17 },
                new RefreshToken { TokenId = 18, Token = "refresh_token_18", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 18 },
                new RefreshToken { TokenId = 19, Token = "refresh_token_19", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 19 },
                new RefreshToken { TokenId = 20, Token = "refresh_token_20", ExpiryDate = DateTime.Now.AddDays(30), CreatedDate = DateTime.Now, AccountId = 20 }
            );
        }
    }
}