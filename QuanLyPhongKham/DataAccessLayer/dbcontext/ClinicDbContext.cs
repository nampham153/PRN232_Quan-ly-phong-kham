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
        public DbSet<TestResultHistory> TestResultHistories { get; set; }

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

            modelBuilder.Entity<TestResultHistory>()
                .HasKey(h => h.Id);
            modelBuilder.Entity<TestResultHistory>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TestResultHistory>()
                .HasOne<TestResult>()
                .WithMany()
                .HasForeignKey(h => h.TestResultId)
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

            // Seed Accounts (120 accounts - 1 admin, 30 doctors, 9 staff, 80 patients)
            var accounts = new List<Account>();

            // Admin account
            accounts.Add(new Account { AccountId = 1, Username = "admin1", PasswordHash = "hashed_password_1", RoleId = 1, Status = true });

            // Doctor accounts (30)
            for (int i = 2; i <= 31; i++)
            {
                accounts.Add(new Account { AccountId = i, Username = $"doctor{i - 1}", PasswordHash = $"hashed_password_{i}", RoleId = 2, Status = true });
            }

            // Staff accounts (9)
            for (int i = 32; i <= 40; i++)
            {
                accounts.Add(new Account { AccountId = i, Username = $"staff{i - 31}", PasswordHash = $"hashed_password_{i}", RoleId = 3, Status = true });
            }

            // Patient accounts (80)
            for (int i = 41; i <= 120; i++)
            {
                accounts.Add(new Account { AccountId = i, Username = $"patient{i - 40}", PasswordHash = $"hashed_password_{i}", RoleId = 4, Status = true });
            }

            modelBuilder.Entity<Account>().HasData(accounts);

            // Seed Users (40 users - 1 admin, 30 doctors, 9 staff)
            var users = new List<User>();
            var doctorNames = new string[] {
                "Dr. Nguyen Van A", "Dr. Le Thi B", "Dr. Tran Van C", "Dr. Pham Minh D", "Dr. Hoang Thi E",
                "Dr. Vu Van F", "Dr. Dang Thi G", "Dr. Bui Van H", "Dr. Do Thi I", "Dr. Ngo Van J",
                "Dr. Ly Minh K", "Dr. Cao Thi L", "Dr. Duong Van M", "Dr. Thai Thi N", "Dr. Lam Van O",
                "Dr. Ha Thi P", "Dr. Vo Van Q", "Dr. Trinh Thi R", "Dr. Dinh Van S", "Dr. Mai Thi T",
                "Dr. Phung Van U", "Dr. Chu Thi V", "Dr. Le Van W", "Dr. Tran Thi X", "Dr. Nguyen Van Y",
                "Dr. Pham Thi Z", "Dr. Hoang Van AA", "Dr. Vu Thi BB", "Dr. Dang Van CC", "Dr. Bui Thi DD"
            };

            var genders = new string[] { "Male", "Female" };
            var random = new Random(123); // Fixed seed for consistent data

            // Admin user
            users.Add(new User { UserId = 1, FullName = "Admin User", Gender = "Male", DOB = new DateTime(1975, 1, 1), Phone = "0901234560", Email = "admin@clinic.com", AccountId = 1 });

            // Doctor users (30)
            for (int i = 2; i <= 31; i++)
            {
                var gender = doctorNames[i - 2].Contains("Thi") ? "Female" : "Male";
                users.Add(new User
                {
                    UserId = i,
                    FullName = doctorNames[i - 2],
                    Gender = gender,
                    DOB = new DateTime(random.Next(1970, 1990), random.Next(1, 13), random.Next(1, 29)),
                    Phone = $"090123{4560 + i}",
                    Email = $"doctor{i - 1}@clinic.com",
                    AccountId = i
                });
            }

            // Staff users (9)
            for (int i = 32; i <= 40; i++)
            {
                var gender = genders[random.Next(2)];
                users.Add(new User
                {
                    UserId = i,
                    FullName = $"Staff {(char)('A' + i - 32)} {(gender == "Male" ? "Van" : "Thi")} {(char)('K' + i - 32)}",
                    Gender = gender,
                    DOB = new DateTime(random.Next(1985, 1995), random.Next(1, 13), random.Next(1, 29)),
                    Phone = $"090123{4560 + i}",
                    Email = $"staff{i - 31}@clinic.com",
                    AccountId = i
                });
            }

            modelBuilder.Entity<User>().HasData(users);

            // Seed Patients (80 patients)
            var patients = new List<Patient>();
            var patientFirstNames = new string[] { "Nguyen", "Tran", "Le", "Pham", "Hoang", "Vu", "Dang", "Bui", "Do", "Ngo", "Ly", "Cao", "Duong", "Thai", "Lam", "Ha", "Vo", "Trinh", "Dinh", "Mai" };
            var patientMiddleNames = new string[] { "Van", "Thi", "Minh", "Thanh", "Duc", "Huu", "Quang", "Anh" };
            var patientLastNames = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T" };
            var diseases = new string[] {
                "None", "Hypertension", "Type 2 Diabetes", "Asthma", "Gastritis", "Arthritis",
                "Allergic Rhinitis", "Migraine", "Depression", "Anxiety", "COPD", "Heart Disease",
                "Kidney Disease", "Liver Disease", "Thyroid Disorder", "Osteoporosis"
            };
            var cities = new string[] { "Hanoi", "Ho Chi Minh", "Da Nang", "Hue", "Can Tho", "Hai Phong", "Nha Trang", "Vung Tau", "Quy Nhon", "Buon Ma Thuot" };

            for (int i = 1; i <= 80; i++)
            {
                var gender = random.Next(2) == 0 ? "Male" : "Female";
                var middleName = gender == "Male" ? "Van" : "Thi";
                var firstName = patientFirstNames[random.Next(patientFirstNames.Length)];
                var lastName = patientLastNames[random.Next(patientLastNames.Length)];
                var disease = diseases[random.Next(diseases.Length)];
                var city = cities[random.Next(cities.Length)];

                patients.Add(new Patient
                {
                    PatientId = i,
                    FullName = $"{firstName} {middleName} {lastName}{i}",
                    Gender = gender,
                    DOB = new DateTime(random.Next(1960, 2000), random.Next(1, 13), random.Next(1, 29)),
                    Phone = $"098765432{i % 10}",
                    Email = $"patient{i}@email.com",
                    Address = $"{random.Next(1, 999)} {firstName} Street, {city}",
                    AccountId = i + 40,
                    AvatarPath = $"/images/avatars/patient{i}.jpg",
                    UnderlyingDiseases = disease,
                    DiseaseDetails = disease == "None" ? "No significant chronic conditions." : $"Diagnosed with {disease.ToLower()}, managed with appropriate treatment."
                });
            }

            modelBuilder.Entity<Patient>().HasData(patients);

            // Seed Tests (50 tests)
            var tests = new List<Test>();
            var testNames = new string[] {
                "Blood Test", "X-Ray", "ECG", "Ultrasound", "CT Scan", "MRI", "Urine Test", "Blood Sugar", "Cholesterol", "Blood Pressure",
                "Liver Function", "Kidney Function", "Thyroid Test", "Vitamin D", "Hemoglobin A1C", "PSA Test", "Mammography", "Colonoscopy",
                "Endoscopy", "Spirometry", "Bone Density", "Stress Test", "Holter Monitor", "Echocardiogram", "Biopsy", "Allergy Test",
                "HIV Test", "Hepatitis Panel", "Tuberculosis Test", "Pap Smear", "Eye Exam", "Hearing Test", "Skin Test", "Pregnancy Test",
                "Stool Test", "Sputum Test", "Culture Test", "Genetic Test", "Cardiac Enzymes", "Tumor Markers", "Coagulation Panel",
                "Electrolytes", "Arterial Blood Gas", "Lactate", "Troponin", "BNP", "D-Dimer", "ESR", "CRP", "Procalcitonin"
            };

            for (int i = 1; i <= 50; i++)
            {
                tests.Add(new Test
                {
                    TestId = i,
                    TestName = testNames[i - 1],
                    Description = $"Medical examination: {testNames[i - 1].ToLower()}"
                });
            }

            modelBuilder.Entity<Test>().HasData(tests);

            // Seed Medicines (100 medicines)
            var medicines = new List<Medicine>();
            var medicineNames = new string[] {
                "Paracetamol", "Amoxicillin", "Ibuprofen", "Aspirin", "Metformin", "Lisinopril", "Omeprazole", "Salbutamol", "Diazepam", "Cetirizine",
                "Atorvastatin", "Losartan", "Amlodipine", "Simvastatin", "Furosemide", "Warfarin", "Insulin", "Prednisone", "Albuterol", "Hydrochlorothiazide",
                "Gabapentin", "Tramadol", "Morphine", "Codeine", "Dexamethasone", "Ciprofloxacin", "Azithromycin", "Cephalexin", "Clindamycin", "Doxycycline",
                "Fluoxetine", "Sertraline", "Alprazolam", "Lorazepam", "Zolpidem", "Trazodone", "Mirtazapine", "Venlafaxine", "Duloxetine", "Bupropion",
                "Levothyroxine", "Methimazole", "Propranolol", "Metoprolol", "Carvedilol", "Digoxin", "Nitroglycerin", "Isosorbide", "Clopidogrel", "Heparin",
                "Ranitidine", "Lansoprazole", "Pantoprazole", "Sucralfate", "Metoclopramide", "Ondansetron", "Loperamide", "Bisacodyl", "Lactulose", "Psyllium",
                "Montelukast", "Loratadine", "Fexofenadine", "Diphenhydramine", "Pseudoephedrine", "Guaifenesin", "Dextromethorphan", "Benzonatate", "Ipratropium", "Budesonide",
                "Calcium", "Vitamin D", "Iron", "Folic Acid", "Vitamin B12", "Magnesium", "Potassium", "Zinc", "Selenium", "Multivitamin",
                "Acetaminophen", "Naproxen", "Meloxicam", "Celecoxib", "Indomethacin", "Ketorolac", "Diclofenac", "Piroxicam", "Sulindac", "Etodolac",
                "Clonazepam", "Phenytoin", "Carbamazepine", "Valproic Acid", "Lamotrigine", "Levetiracetam", "Topiramate", "Oxcarbazepine", "Pregabalin", "Baclofen"
            };

            var units = new string[] { "tablet", "capsule", "ml", "mg", "inhaler", "injection", "cream", "drops" };

            for (int i = 1; i <= 100; i++)
            {
                var unit = units[random.Next(units.Length)];
                medicines.Add(new Medicine
                {
                    MedicineId = i,
                    MedicineName = medicineNames[i - 1],
                    Unit = unit,
                    Usage = $"Take as prescribed by doctor - {unit} form"
                });
            }

            modelBuilder.Entity<Medicine>().HasData(medicines);

            // Seed Medical Records (200 records)
            var medicalRecords = new List<MedicalRecord>();
            var symptoms = new string[] {
                "Fever, headache", "Chest pain", "Shortness of breath", "High blood pressure", "Stomach pain",
                "Joint pain", "Skin rash", "Diabetes symptoms", "Anxiety", "Allergic rhinitis", "Fatigue",
                "Dizziness", "Nausea", "Vomiting", "Diarrhea", "Constipation", "Back pain", "Neck pain",
                "Muscle pain", "Insomnia", "Depression", "Cough", "Sore throat", "Runny nose", "Ear pain"
            };

            var diagnoses = new string[] {
                "Common cold", "Muscle strain", "Asthma", "Hypertension", "Gastritis", "Arthritis",
                "Allergic reaction", "Type 2 Diabetes", "Anxiety disorder", "Hay fever", "Viral infection",
                "Bacterial infection", "Migraine", "Tension headache", "GERD", "IBS", "UTI", "Bronchitis",
                "Pneumonia", "Flu", "Food poisoning", "Dermatitis", "Osteoarthritis", "Fibromyalgia", "Sinusitis"
            };

            for (int i = 1; i <= 200; i++)
            {
                var patientId = random.Next(1, 81);
                var doctorId = random.Next(2, 32);
                var symptom = symptoms[random.Next(symptoms.Length)];
                var diagnosis = diagnoses[random.Next(diagnoses.Length)];

                medicalRecords.Add(new MedicalRecord
                {
                    RecordId = i,
                    PatientId = patientId,
                    UserId = doctorId,
                    Date = DateTime.Now.AddDays(-random.Next(1, 365)),
                    Symptoms = symptom,
                    Diagnosis = diagnosis,
                    Note = $"Treatment plan recommended for {diagnosis.ToLower()}"
                });
            }

            modelBuilder.Entity<MedicalRecord>().HasData(medicalRecords);

            // Seed Test Results (300 results)
            var testResults = new List<TestResult>();
            var resultDetails = new string[] {
                "Normal range", "Slightly elevated", "Within normal limits", "Abnormal findings detected",
                "Requires follow-up", "Improved from previous", "No significant changes", "Critical values noted"
            };

            for (int i = 1; i <= 300; i++)
            {
                var recordId = random.Next(1, 201);
                var testId = random.Next(1, 51);
                var doctorId = random.Next(2, 32);
                var resultDetail = resultDetails[random.Next(resultDetails.Length)];

                testResults.Add(new TestResult
                {
                    ResultId = i,
                    RecordId = recordId,
                    TestId = testId,
                    UserId = doctorId,
                    ResultDetail = $"{resultDetail} - Test #{testId}",
                    TestDate = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }

            modelBuilder.Entity<TestResult>().HasData(testResults);

            // Seed Test Result Histories (300 histories)
            var testResultHistories = new List<TestResultHistory>();
            var actions = new string[] { "Create", "Update", "Review", "Approve", "Cancel" };

            for (int i = 1; i <= 300; i++)
            {
                var userId = random.Next(2, 32);
                var action = actions[random.Next(actions.Length)];

                testResultHistories.Add(new TestResultHistory
                {
                    Id = i,
                    UserId = userId,
                    TestResultId = i,
                    Action = action,
                    ActionTime = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }

            modelBuilder.Entity<TestResultHistory>().HasData(testResultHistories);

            // Seed Prescriptions (400 prescriptions)
            var prescriptions = new List<Prescription>();
            var dosages = new string[] {
                "Once daily", "Twice daily", "Three times daily", "Four times daily", "As needed",
                "Every 8 hours", "Every 12 hours", "Before meals", "After meals", "At bedtime"
            };

            for (int i = 1; i <= 400; i++)
            {
                var recordId = random.Next(1, 201);
                var medicineId = random.Next(1, 101);
                var quantity = random.Next(10, 100);
                var dosage = dosages[random.Next(dosages.Length)];

                prescriptions.Add(new Prescription
                {
                    PrescriptionId = i,
                    RecordId = recordId,
                    MedicineId = medicineId,
                    Quantity = quantity,
                    Dosage = dosage,
                    Date = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }

            modelBuilder.Entity<Prescription>().HasData(prescriptions);

            // Seed Refresh Tokens (120 tokens - one for each account)
            var refreshTokens = new List<RefreshToken>();

            for (int i = 1; i <= 120; i++)
            {
                refreshTokens.Add(new RefreshToken
                {
                    TokenId = i,
                    Token = $"refresh_token_{i}_{Guid.NewGuid().ToString().Substring(0, 8)}",
                    ExpiryDate = DateTime.Now.AddDays(30),
                    CreatedDate = DateTime.Now.AddDays(-random.Next(0, 30)),
                    AccountId = i
                });
            }

            modelBuilder.Entity<RefreshToken>().HasData(refreshTokens);
        }
    }
}