using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class inititaldb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    MedicineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.MedicineId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    TestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.TestId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    AvatarPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patients_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    DoctorPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Symptoms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_MedicalRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "MedicineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResultDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_TestResults_MedicalRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestResultHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TestResultId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResultHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestResultHistories_TestResults_TestResultId",
                        column: x => x.TestResultId,
                        principalTable: "TestResults",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TestResultHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "MedicineId", "MedicineName", "Unit", "Usage" },
                values: new object[,]
                {
                    { 1, "Paracetamol", "tablet", "Take 1-2 tablets every 4-6 hours" },
                    { 2, "Amoxicillin", "capsule", "Take 1 capsule 3 times daily" },
                    { 3, "Ibuprofen", "tablet", "Take 1 tablet every 8 hours" },
                    { 4, "Aspirin", "tablet", "Take 1 tablet daily" },
                    { 5, "Metformin", "tablet", "Take 1 tablet twice daily with meals" },
                    { 6, "Lisinopril", "tablet", "Take 1 tablet once daily" },
                    { 7, "Omeprazole", "capsule", "Take 1 capsule before breakfast" },
                    { 8, "Salbutamol", "inhaler", "2 puffs when needed" },
                    { 9, "Diazepam", "tablet", "Take 1 tablet when needed" },
                    { 10, "Cetirizine", "tablet", "Take 1 tablet once daily" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Doctor" },
                    { 3, "Staff" },
                    { 4, "Patient" }
                });

            migrationBuilder.InsertData(
                table: "Tests",
                columns: new[] { "TestId", "Description", "TestName" },
                values: new object[,]
                {
                    { 1, "Complete blood count analysis", "Blood Test" },
                    { 2, "Chest X-ray examination", "X-Ray" },
                    { 3, "Electrocardiogram test", "ECG" },
                    { 4, "Abdominal ultrasound", "Ultrasound" },
                    { 5, "Computed tomography scan", "CT Scan" },
                    { 6, "Magnetic resonance imaging", "MRI" },
                    { 7, "Urinalysis examination", "Urine Test" },
                    { 8, "Glucose level test", "Blood Sugar" },
                    { 9, "Lipid profile test", "Cholesterol" },
                    { 10, "Hypertension screening", "Blood Pressure" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "PasswordHash", "RoleId", "Status", "Username" },
                values: new object[,]
                {
                    { 1, "hashed_password_1", 1, true, "admin1" },
                    { 2, "hashed_password_2", 2, true, "doctor1" },
                    { 3, "hashed_password_3", 2, true, "doctor2" },
                    { 4, "hashed_password_4", 2, true, "doctor3" },
                    { 5, "hashed_password_5", 2, true, "doctor4" },
                    { 6, "hashed_password_6", 2, true, "doctor5" },
                    { 7, "hashed_password_7", 2, true, "doctor6" },
                    { 8, "hashed_password_8", 2, true, "doctor7" },
                    { 9, "hashed_password_9", 2, true, "doctor8" },
                    { 10, "hashed_password_10", 2, true, "doctor9" },
                    { 11, "hashed_password_11", 3, true, "staff1" },
                    { 12, "hashed_password_12", 4, true, "patient1" },
                    { 13, "hashed_password_13", 4, true, "patient2" },
                    { 14, "hashed_password_14", 4, true, "patient3" },
                    { 15, "hashed_password_15", 4, true, "patient4" },
                    { 16, "hashed_password_16", 4, true, "patient5" },
                    { 17, "hashed_password_17", 4, true, "patient6" },
                    { 18, "hashed_password_18", 4, true, "patient7" },
                    { 19, "hashed_password_19", 4, true, "patient8" },
                    { 20, "hashed_password_20", 4, true, "patient9" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "AccountId", "Address", "AvatarPath", "DOB", "Email", "FullName", "Gender", "Phone" },
                values: new object[,]
                {
                    { 1, 12, "123 Nguyen Trai, Hanoi", "/images/avatars/patient1.jpg", new DateTime(1995, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient1@email.com", "Nguyen Thi K", "Female", "0987654321" },
                    { 2, 13, "456 Le Loi, Ho Chi Minh", "/images/avatars/patient2.jpg", new DateTime(1992, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient2@email.com", "Tran Van L", "Male", "0987654322" },
                    { 3, 14, "789 Tran Phu, Da Nang", "/images/avatars/patient3.jpg", new DateTime(1988, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient3@email.com", "Le Thi M", "Female", "0987654323" },
                    { 4, 15, "321 Hai Ba Trung, Hue", "/images/avatars/patient4.jpg", new DateTime(1990, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient4@email.com", "Hoang Van N", "Male", "0987654324" },
                    { 5, 16, "654 Dong Khoi, Can Tho", "/images/avatars/patient5.jpg", new DateTime(1985, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient5@email.com", "Pham Thi O", "Female", "0987654325" },
                    { 6, 17, "987 Bach Dang, Hai Phong", "/images/avatars/patient6.jpg", new DateTime(1993, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient6@email.com", "Vu Van P", "Male", "0987654326" },
                    { 7, 18, "147 Ly Thuong Kiet, Nha Trang", "/images/avatars/patient7.jpg", new DateTime(1987, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient7@email.com", "Dang Thi Q", "Female", "0987654327" },
                    { 8, 19, "258 Quang Trung, Vung Tau", "/images/avatars/patient8.jpg", new DateTime(1991, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient8@email.com", "Bui Van R", "Male", "0987654328" },
                    { 9, 20, "369 Le Duan, Dalat", "/images/avatars/patient9.jpg", new DateTime(1989, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "patient9@email.com", "Do Thi S", "Female", "0987654329" }
                });

            migrationBuilder.InsertData(
                table: "RefreshTokens",
                columns: new[] { "TokenId", "AccountId", "CreatedDate", "ExpiryDate", "Token" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7742), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7741), "refresh_token_1" },
                    { 2, 2, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7745), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7745), "refresh_token_2" },
                    { 3, 3, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7747), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7746), "refresh_token_3" },
                    { 4, 4, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7748), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7748), "refresh_token_4" },
                    { 5, 5, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7750), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7749), "refresh_token_5" },
                    { 6, 6, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7751), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7751), "refresh_token_6" },
                    { 7, 7, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7753), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7752), "refresh_token_7" },
                    { 8, 8, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7754), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7753), "refresh_token_8" },
                    { 9, 9, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7755), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7755), "refresh_token_9" },
                    { 10, 10, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7757), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7756), "refresh_token_10" },
                    { 11, 11, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7759), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7758), "refresh_token_11" },
                    { 12, 12, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7760), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7760), "refresh_token_12" },
                    { 13, 13, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7762), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7761), "refresh_token_13" },
                    { 14, 14, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7763), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7763), "refresh_token_14" },
                    { 15, 15, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7764), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7764), "refresh_token_15" },
                    { 16, 16, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7766), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7765), "refresh_token_16" },
                    { 17, 17, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7767), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7767), "refresh_token_17" },
                    { 18, 18, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7769), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7768), "refresh_token_18" },
                    { 19, 19, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7770), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7770), "refresh_token_19" },
                    { 20, 20, new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7772), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7771), "refresh_token_20" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccountId", "DOB", "DoctorPath", "Email", "FullName", "Gender", "Phone" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1975, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@clinic.com", "Admin User", "Male", "0901234560" },
                    { 2, 2, new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor1@clinic.com", "Dr. Nguyen Van A", "Male", "0901234567" },
                    { 3, 3, new DateTime(1985, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor2@clinic.com", "Dr. Le Thi B", "Female", "0901234568" },
                    { 4, 4, new DateTime(1978, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor3@clinic.com", "Dr. Tran Van C", "Male", "0901234570" },
                    { 5, 5, new DateTime(1982, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor4@clinic.com", "Dr. Pham Minh D", "Male", "0901234571" },
                    { 6, 6, new DateTime(1975, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor5@clinic.com", "Dr. Hoang Thi E", "Female", "0901234572" },
                    { 7, 7, new DateTime(1988, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor6@clinic.com", "Dr. Vu Van F", "Male", "0901234573" },
                    { 8, 8, new DateTime(1983, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor7@clinic.com", "Dr. Dang Thi G", "Female", "0901234574" },
                    { 9, 9, new DateTime(1987, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor8@clinic.com", "Dr. Bui Van H", "Male", "0901234575" },
                    { 10, 10, new DateTime(1981, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor9@clinic.com", "Dr. Do Thi I", "Female", "0901234576" },
                    { 11, 11, new DateTime(1990, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "staff1@clinic.com", "Staff Ngo Van J", "Male", "0901234569" }
                });

            migrationBuilder.InsertData(
                table: "MedicalRecords",
                columns: new[] { "RecordId", "Date", "Diagnosis", "Note", "PatientId", "Symptoms", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7561), "Common cold", "Rest and fluids recommended", 1, "Fever, headache", 2 },
                    { 2, new DateTime(2025, 6, 26, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7582), "Muscle strain", "Apply heat therapy", 2, "Chest pain", 2 },
                    { 3, new DateTime(2025, 7, 1, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7583), "Asthma", "Prescribed inhaler", 3, "Shortness of breath", 3 },
                    { 4, new DateTime(2025, 7, 6, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7585), "Hypertension", "Lifestyle changes needed", 4, "High blood pressure", 3 },
                    { 5, new DateTime(2025, 7, 11, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7586), "Gastritis", "Avoid spicy foods", 5, "Stomach pain", 4 },
                    { 6, new DateTime(2025, 7, 13, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7587), "Arthritis", "Physical therapy recommended", 6, "Joint pain", 4 },
                    { 7, new DateTime(2025, 7, 16, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7589), "Allergic reaction", "Avoid allergens", 7, "Skin rash", 5 },
                    { 8, new DateTime(2025, 7, 18, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7590), "Type 2 Diabetes", "Diet control important", 8, "Diabetes symptoms", 5 },
                    { 9, new DateTime(2025, 7, 19, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7592), "Anxiety disorder", "Counseling recommended", 9, "Anxiety", 6 },
                    { 10, new DateTime(2025, 7, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7593), "Hay fever", "Seasonal allergy", 1, "Allergic rhinitis", 6 }
                });

            migrationBuilder.InsertData(
                table: "Prescriptions",
                columns: new[] { "PrescriptionId", "Dosage", "MedicineId", "Quantity", "RecordId" },
                values: new object[,]
                {
                    { 1, "500mg twice daily", 1, 20, 1 },
                    { 2, "400mg three times daily", 3, 15, 2 },
                    { 3, "2 puffs as needed", 8, 1, 3 },
                    { 4, "10mg once daily", 6, 30, 4 },
                    { 5, "20mg before breakfast", 7, 14, 5 },
                    { 6, "200mg twice daily", 3, 30, 6 },
                    { 7, "10mg once daily", 10, 10, 7 },
                    { 8, "500mg twice daily", 5, 60, 8 },
                    { 9, "2mg as needed", 9, 10, 9 },
                    { 10, "10mg once daily", 10, 30, 10 }
                });

            migrationBuilder.InsertData(
                table: "TestResults",
                columns: new[] { "ResultId", "RecordId", "ResultDetail", "TestDate", "TestId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "WBC: 8.5, RBC: 4.2", new DateTime(2025, 6, 22, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7620), 1, 2 },
                    { 2, 2, "Chest clear, no abnormalities", new DateTime(2025, 6, 27, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7622), 2, 2 },
                    { 3, 3, "Normal heart rhythm", new DateTime(2025, 7, 2, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7623), 3, 3 },
                    { 4, 4, "BP: 140/90 mmHg", new DateTime(2025, 7, 7, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7624), 10, 3 },
                    { 5, 5, "Mild gastric inflammation", new DateTime(2025, 7, 12, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7626), 4, 4 },
                    { 6, 6, "Joint space narrowing", new DateTime(2025, 7, 14, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7627), 2, 4 },
                    { 7, 7, "Elevated eosinophils", new DateTime(2025, 7, 17, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7628), 1, 5 },
                    { 8, 8, "Glucose: 180 mg/dL", new DateTime(2025, 7, 19, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7630), 8, 5 },
                    { 9, 9, "Normal blood parameters", new DateTime(2025, 7, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7631), 1, 6 },
                    { 10, 10, "Increased histamine levels", new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7632), 7, 6 }
                });

            migrationBuilder.InsertData(
                table: "TestResultHistories",
                columns: new[] { "Id", "Action", "ActionTime", "Note", "TestResultId", "UserId" },
                values: new object[,]
                {
                    { 1, "Create", new DateTime(2025, 6, 22, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7653), null, 1, 2 },
                    { 2, "Create", new DateTime(2025, 6, 27, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7654), null, 2, 2 },
                    { 3, "Create", new DateTime(2025, 7, 2, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7655), null, 3, 3 },
                    { 4, "Create", new DateTime(2025, 7, 7, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7656), null, 4, 3 },
                    { 5, "Create", new DateTime(2025, 7, 12, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7658), null, 5, 4 },
                    { 6, "Create", new DateTime(2025, 7, 14, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7659), null, 6, 4 },
                    { 7, "Create", new DateTime(2025, 7, 17, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7660), null, 7, 5 },
                    { 8, "Create", new DateTime(2025, 7, 19, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7661), null, 8, 5 },
                    { 9, "Create", new DateTime(2025, 7, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7662), null, 9, 6 },
                    { 10, "Create", new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7663), null, 10, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RoleId",
                table: "Accounts",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_PatientId",
                table: "MedicalRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_UserId",
                table: "MedicalRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AccountId",
                table: "Patients",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicineId",
                table: "Prescriptions",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_RecordId",
                table: "Prescriptions",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccountId",
                table: "RefreshTokens",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestResultHistories_TestResultId",
                table: "TestResultHistories",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResultHistories_UserId",
                table: "TestResultHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_RecordId",
                table: "TestResults",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestId",
                table: "TestResults",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountId",
                table: "Users",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TestResultHistories");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
