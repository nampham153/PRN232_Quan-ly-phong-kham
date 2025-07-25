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
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsCheck = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    AvatarPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnderlyingDiseases = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiseaseDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                columns: new[] { "MedicineId", "CreatedDate", "MedicineName", "Unit", "Usage" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paracetamol", "tablet", "Take 1-2 tablets every 4-6 hours" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amoxicillin", "capsule", "Take 1 capsule 3 times daily" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ibuprofen", "tablet", "Take 1 tablet every 8 hours" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aspirin", "tablet", "Take 1 tablet daily" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Metformin", "tablet", "Take 1 tablet twice daily with meals" },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lisinopril", "tablet", "Take 1 tablet once daily" },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omeprazole", "capsule", "Take 1 capsule before breakfast" },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salbutamol", "inhaler", "2 puffs when needed" },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diazepam", "tablet", "Take 1 tablet when needed" },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cetirizine", "tablet", "Take 1 tablet once daily" }
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
                columns: new[] { "AccountId", "CreatedAt", "IsCheck", "PasswordHash", "RoleId", "Status", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5207), true, "hashed_password_1", 1, true, null, "admin1" },
                    { 2, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5213), true, "hashed_password_2", 2, true, null, "doctor1" },
                    { 3, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5214), true, "hashed_password_3", 2, true, null, "doctor2" },
                    { 4, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5215), true, "hashed_password_4", 2, true, null, "doctor3" },
                    { 5, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5216), true, "hashed_password_5", 2, true, null, "doctor4" },
                    { 6, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5217), true, "hashed_password_6", 2, true, null, "doctor5" },
                    { 7, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5218), true, "hashed_password_7", 2, true, null, "doctor6" },
                    { 8, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5219), true, "hashed_password_8", 2, true, null, "doctor7" },
                    { 9, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5220), true, "hashed_password_9", 2, true, null, "doctor8" },
                    { 10, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5221), true, "hashed_password_10", 2, true, null, "doctor9" },
                    { 11, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5222), true, "hashed_password_11", 2, true, null, "doctor10" },
                    { 12, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5223), true, "hashed_password_12", 3, true, null, "staff1" },
                    { 13, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5224), true, "hashed_password_13", 4, true, null, "patient1" },
                    { 14, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5225), true, "hashed_password_14", 4, true, null, "patient2" },
                    { 15, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5226), true, "hashed_password_15", 4, true, null, "patient3" },
                    { 16, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5226), true, "hashed_password_16", 4, true, null, "patient4" },
                    { 17, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5227), true, "hashed_password_17", 4, true, null, "patient5" },
                    { 18, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5228), true, "hashed_password_18", 4, true, null, "patient6" },
                    { 19, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5229), true, "hashed_password_19", 4, true, null, "patient7" },
                    { 20, new DateTime(2025, 7, 25, 14, 58, 46, 500, DateTimeKind.Utc).AddTicks(5230), true, "hashed_password_20", 4, true, null, "patient8" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "AccountId", "Address", "AvatarPath", "DOB", "DiseaseDetails", "Email", "FullName", "Gender", "Phone", "UnderlyingDiseases" },
                values: new object[,]
                {
                    { 1, 13, "123 Nguyen Trai, Hanoi", "/images/avatars/patient1.jpg", new DateTime(1995, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diagnosed with hypertension in 2020, managed with lifestyle changes and medication.", "patient1@email.com", "Nguyen Thi K", "Female", "0987654321", "Hypertension" },
                    { 2, 14, "456 Le Loi, Ho Chi Minh", "/images/avatars/patient2.jpg", new DateTime(1992, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "No significant chronic conditions, occasional muscle strain from physical activity.", "patient2@email.com", "Tran Van L", "Male", "0987654322", "None" },
                    { 3, 15, "789 Tran Phu, Da Nang", "/images/avatars/patient3.jpg", new DateTime(1988, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diagnosed with asthma at age 15, uses inhaler as needed.", "patient3@email.com", "Le Thi M", "Female", "0987654323", "Asthma" },
                    { 4, 16, "321 Hai Ba Trung, Hue", "/images/avatars/patient4.jpg", new DateTime(1990, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hypertension since 2018, Type 2 Diabetes diagnosed in 2021, on metformin.", "patient4@email.com", "Hoang Van N", "Male", "0987654324", "Hypertension, Type 2 Diabetes" },
                    { 5, 17, "654 Dong Khoi, Can Tho", "/images/avatars/patient5.jpg", new DateTime(1985, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chronic gastritis diagnosed in 2019, managed with PPI and diet.", "patient5@email.com", "Pham Thi O", "Female", "0987654325", "Gastritis" },
                    { 6, 18, "987 Bach Dang, Hai Phong", "/images/avatars/patient6.jpg", new DateTime(1993, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rheumatoid arthritis diagnosed in 2022, on anti-inflammatory medication.", "patient6@email.com", "Vu Van P", "Male", "0987654326", "Arthritis" },
                    { 7, 19, "147 Ly Thuong Kiet, Nha Trang", "/images/avatars/patient7.jpg", new DateTime(1987, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seasonal allergies since childhood, managed with antihistamines.", "patient7@email.com", "Dang Thi Q", "Female", "0987654327", "Allergic Rhinitis" },
                    { 8, 20, "258 Quang Trung, Vung Tau", "/images/avatars/patient8.jpg", new DateTime(1991, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diagnosed with Type 2 Diabetes in 2020, controlled with metformin and diet.", "patient8@email.com", "Bui Van R", "Male", "0987654328", "Type 2 Diabetes" }
                });

            migrationBuilder.InsertData(
                table: "RefreshTokens",
                columns: new[] { "TokenId", "AccountId", "CreatedDate", "ExpiryDate", "Token" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5764), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5764), "refresh_token_1" },
                    { 2, 2, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5767), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5766), "refresh_token_2" },
                    { 3, 3, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5768), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5768), "refresh_token_3" },
                    { 4, 4, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5770), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5769), "refresh_token_4" },
                    { 5, 5, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5771), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5771), "refresh_token_5" },
                    { 6, 6, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5773), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5772), "refresh_token_6" },
                    { 7, 7, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5774), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5774), "refresh_token_7" },
                    { 8, 8, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5776), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5775), "refresh_token_8" },
                    { 9, 9, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5777), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5777), "refresh_token_9" },
                    { 10, 10, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5779), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5778), "refresh_token_10" },
                    { 11, 11, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5780), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5780), "refresh_token_11" },
                    { 12, 12, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5782), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5781), "refresh_token_12" },
                    { 13, 13, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5783), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5783), "refresh_token_13" },
                    { 14, 14, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5785), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5784), "refresh_token_14" },
                    { 15, 15, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5786), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5786), "refresh_token_15" },
                    { 16, 16, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5788), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5787), "refresh_token_16" },
                    { 17, 17, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5789), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5789), "refresh_token_17" },
                    { 18, 18, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5791), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5790), "refresh_token_18" },
                    { 19, 19, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5792), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5792), "refresh_token_19" },
                    { 20, 20, new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5794), new DateTime(2025, 8, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5793), "refresh_token_20" }
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
                    { 11, 11, new DateTime(1984, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "doctor10@clinic.com", "Dr. Ngo Van J", "Male", "0901234577" },
                    { 12, 12, new DateTime(1990, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "staff1@clinic.com", "Staff Ngo Van K", "Male", "0901234569" }
                });

            migrationBuilder.InsertData(
                table: "MedicalRecords",
                columns: new[] { "RecordId", "Date", "Diagnosis", "Note", "PatientId", "Symptoms", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5561), "Common cold", "Rest and fluids recommended", 1, "Fever, headache", 2 },
                    { 2, new DateTime(2025, 6, 30, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5584), "Muscle strain", "Apply heat therapy", 2, "Chest pain", 3 },
                    { 3, new DateTime(2025, 7, 5, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5586), "Asthma", "Prescribed inhaler", 3, "Shortness of breath", 4 },
                    { 4, new DateTime(2025, 7, 10, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5588), "Hypertension", "Lifestyle changes needed", 4, "High blood pressure", 5 },
                    { 5, new DateTime(2025, 7, 15, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5589), "Gastritis", "Avoid spicy foods", 5, "Stomach pain", 6 },
                    { 6, new DateTime(2025, 7, 17, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5591), "Arthritis", "Physical therapy recommended", 6, "Joint pain", 7 },
                    { 7, new DateTime(2025, 7, 20, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5593), "Allergic reaction", "Avoid allergens", 7, "Skin rash", 8 },
                    { 8, new DateTime(2025, 7, 22, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5594), "Type 2 Diabetes", "Diet control important", 8, "Diabetes symptoms", 9 },
                    { 9, new DateTime(2025, 7, 23, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5596), "Anxiety disorder", "Counseling recommended", 1, "Anxiety", 10 },
                    { 10, new DateTime(2025, 7, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5597), "Hay fever", "Seasonal allergy", 2, "Allergic rhinitis", 11 }
                });

            migrationBuilder.InsertData(
                table: "Prescriptions",
                columns: new[] { "PrescriptionId", "Date", "Dosage", "MedicineId", "Quantity", "RecordId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5708), "500mg twice daily", 1, 20, 1 },
                    { 2, new DateTime(2025, 6, 30, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5709), "400mg three times daily", 3, 15, 2 },
                    { 3, new DateTime(2025, 7, 5, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5711), "2 puffs as needed", 8, 1, 3 },
                    { 4, new DateTime(2025, 7, 10, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5713), "10mg once daily", 6, 30, 4 },
                    { 5, new DateTime(2025, 7, 15, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5714), "20mg before breakfast", 7, 14, 5 },
                    { 6, new DateTime(2025, 7, 17, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5715), "200mg twice daily", 3, 30, 6 },
                    { 7, new DateTime(2025, 7, 20, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5716), "10mg once daily", 10, 10, 7 },
                    { 8, new DateTime(2025, 7, 22, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5718), "500mg twice daily", 5, 60, 8 },
                    { 9, new DateTime(2025, 7, 23, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5719), "2mg as needed", 9, 10, 9 },
                    { 10, new DateTime(2025, 7, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5738), "10mg once daily", 10, 30, 10 }
                });

            migrationBuilder.InsertData(
                table: "TestResults",
                columns: new[] { "ResultId", "RecordId", "ResultDetail", "TestDate", "TestId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "WBC: 8.5, RBC: 4.2", new DateTime(2025, 6, 26, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5625), 1, 2 },
                    { 2, 2, "Chest clear, no abnormalities", new DateTime(2025, 7, 1, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5627), 2, 3 },
                    { 3, 3, "Normal heart rhythm", new DateTime(2025, 7, 6, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5628), 3, 4 },
                    { 4, 4, "BP: 140/90 mmHg", new DateTime(2025, 7, 11, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5630), 10, 5 },
                    { 5, 5, "Mild gastric inflammation", new DateTime(2025, 7, 16, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5631), 4, 6 },
                    { 6, 6, "Joint space narrowing", new DateTime(2025, 7, 18, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5632), 2, 7 },
                    { 7, 7, "Elevated eosinophils", new DateTime(2025, 7, 21, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5633), 1, 8 },
                    { 8, 8, "Glucose: 180 mg/dL", new DateTime(2025, 7, 23, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5634), 8, 9 },
                    { 9, 9, "Normal blood parameters", new DateTime(2025, 7, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5636), 1, 10 },
                    { 10, 10, "Increased histamine levels", new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5637), 7, 11 }
                });

            migrationBuilder.InsertData(
                table: "TestResultHistories",
                columns: new[] { "Id", "Action", "ActionTime", "Note", "TestResultId", "UserId" },
                values: new object[,]
                {
                    { 1, "Create", new DateTime(2025, 6, 26, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5660), null, 1, 2 },
                    { 2, "Create", new DateTime(2025, 7, 1, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5662), null, 2, 3 },
                    { 3, "Create", new DateTime(2025, 7, 6, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5663), null, 3, 4 },
                    { 4, "Create", new DateTime(2025, 7, 11, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5664), null, 4, 5 },
                    { 5, "Create", new DateTime(2025, 7, 16, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5665), null, 5, 6 },
                    { 6, "Create", new DateTime(2025, 7, 18, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5667), null, 6, 7 },
                    { 7, "Create", new DateTime(2025, 7, 21, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5668), null, 7, 8 },
                    { 8, "Create", new DateTime(2025, 7, 23, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5669), null, 8, 9 },
                    { 9, "Create", new DateTime(2025, 7, 24, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5670), null, 9, 10 },
                    { 10, "Create", new DateTime(2025, 7, 25, 21, 58, 46, 500, DateTimeKind.Local).AddTicks(5671), null, 10, 11 }
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
