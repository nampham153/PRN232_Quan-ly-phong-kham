using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateinititalDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Medicines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCheck",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7708), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7714), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7715), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7716), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7717), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7718), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7719), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7720), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7723), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7723), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 11,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7724), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 12,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7725), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 13,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7726), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 14,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7727), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 15,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7728), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 16,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7728), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 17,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7729), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 18,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7730), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 19,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7731), true, null });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: 20,
                columns: new[] { "CreatedAt", "IsCheck", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 9, 48, 774, DateTimeKind.Utc).AddTicks(7732), true, null });

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 6, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7967));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2025, 6, 28, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7991));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2025, 7, 3, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7992));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2025, 7, 8, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7994));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2025, 7, 13, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7995));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 6,
                column: "Date",
                value: new DateTime(2025, 7, 15, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7997));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 7,
                column: "Date",
                value: new DateTime(2025, 7, 18, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(7998));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 8,
                column: "Date",
                value: new DateTime(2025, 7, 20, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8000));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 9,
                column: "Date",
                value: new DateTime(2025, 7, 21, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8001));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 10,
                column: "Date",
                value: new DateTime(2025, 7, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8002));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Medicines",
                keyColumn: "MedicineId",
                keyValue: 10,
                column: "CreatedDate",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8170), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8169) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8172), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8171) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8173), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8173) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8175), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8174) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8176), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8175) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 6,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8177), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8177) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 7,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8179), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8178) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 8,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8180), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8180) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 9,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8181), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8181) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 10,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8183), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8182) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 11,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8184), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8184) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 12,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8186), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8185) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 13,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8187), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8187) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 14,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8189), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8188) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 15,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8190), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8189) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 16,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8191), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8191) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 17,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8193), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8192) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 18,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8194), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8194) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 19,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8195), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8195) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 20,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8197), new DateTime(2025, 8, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8196) });

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ActionTime",
                value: new DateTime(2025, 6, 24, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8068));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ActionTime",
                value: new DateTime(2025, 6, 29, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8077));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ActionTime",
                value: new DateTime(2025, 7, 4, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8078));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ActionTime",
                value: new DateTime(2025, 7, 9, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8079));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 5,
                column: "ActionTime",
                value: new DateTime(2025, 7, 14, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8080));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 6,
                column: "ActionTime",
                value: new DateTime(2025, 7, 16, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8082));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 7,
                column: "ActionTime",
                value: new DateTime(2025, 7, 19, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8083));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 8,
                column: "ActionTime",
                value: new DateTime(2025, 7, 21, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8084));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 9,
                column: "ActionTime",
                value: new DateTime(2025, 7, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8085));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 10,
                column: "ActionTime",
                value: new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8086));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 1,
                column: "TestDate",
                value: new DateTime(2025, 6, 24, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8029));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 2,
                column: "TestDate",
                value: new DateTime(2025, 6, 29, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8031));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 3,
                column: "TestDate",
                value: new DateTime(2025, 7, 4, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8032));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 4,
                column: "TestDate",
                value: new DateTime(2025, 7, 9, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8034));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 5,
                column: "TestDate",
                value: new DateTime(2025, 7, 14, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 6,
                column: "TestDate",
                value: new DateTime(2025, 7, 16, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8038));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 7,
                column: "TestDate",
                value: new DateTime(2025, 7, 19, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8039));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 8,
                column: "TestDate",
                value: new DateTime(2025, 7, 21, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8040));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 9,
                column: "TestDate",
                value: new DateTime(2025, 7, 22, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8042));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 10,
                column: "TestDate",
                value: new DateTime(2025, 7, 23, 22, 9, 48, 774, DateTimeKind.Local).AddTicks(8043));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsCheck",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 6, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7561));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2025, 6, 26, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7582));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 3,
                column: "Date",
                value: new DateTime(2025, 7, 1, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7583));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 4,
                column: "Date",
                value: new DateTime(2025, 7, 6, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7585));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 5,
                column: "Date",
                value: new DateTime(2025, 7, 11, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7586));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 6,
                column: "Date",
                value: new DateTime(2025, 7, 13, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7587));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 7,
                column: "Date",
                value: new DateTime(2025, 7, 16, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7589));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 8,
                column: "Date",
                value: new DateTime(2025, 7, 18, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7590));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 9,
                column: "Date",
                value: new DateTime(2025, 7, 19, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7592));

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "RecordId",
                keyValue: 10,
                column: "Date",
                value: new DateTime(2025, 7, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7593));

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7742), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7741) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7745), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7745) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7747), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7746) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7748), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7748) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7750), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7749) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 6,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7751), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7751) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 7,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7753), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7752) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 8,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7754), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7753) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 9,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7755), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7755) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 10,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7757), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7756) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 11,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7759), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7758) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 12,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7760), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7760) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 13,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7762), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7761) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 14,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7763), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7763) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 15,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7764), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7764) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 16,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7766), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 17,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7767), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7767) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 18,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7769), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7768) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 19,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7770), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7770) });

            migrationBuilder.UpdateData(
                table: "RefreshTokens",
                keyColumn: "TokenId",
                keyValue: 20,
                columns: new[] { "CreatedDate", "ExpiryDate" },
                values: new object[] { new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7772), new DateTime(2025, 8, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7771) });

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 1,
                column: "ActionTime",
                value: new DateTime(2025, 6, 22, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7653));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 2,
                column: "ActionTime",
                value: new DateTime(2025, 6, 27, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7654));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 3,
                column: "ActionTime",
                value: new DateTime(2025, 7, 2, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7655));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 4,
                column: "ActionTime",
                value: new DateTime(2025, 7, 7, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7656));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 5,
                column: "ActionTime",
                value: new DateTime(2025, 7, 12, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7658));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 6,
                column: "ActionTime",
                value: new DateTime(2025, 7, 14, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7659));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 7,
                column: "ActionTime",
                value: new DateTime(2025, 7, 17, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7660));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 8,
                column: "ActionTime",
                value: new DateTime(2025, 7, 19, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7661));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 9,
                column: "ActionTime",
                value: new DateTime(2025, 7, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7662));

            migrationBuilder.UpdateData(
                table: "TestResultHistories",
                keyColumn: "Id",
                keyValue: 10,
                column: "ActionTime",
                value: new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7663));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 1,
                column: "TestDate",
                value: new DateTime(2025, 6, 22, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7620));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 2,
                column: "TestDate",
                value: new DateTime(2025, 6, 27, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7622));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 3,
                column: "TestDate",
                value: new DateTime(2025, 7, 2, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7623));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 4,
                column: "TestDate",
                value: new DateTime(2025, 7, 7, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7624));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 5,
                column: "TestDate",
                value: new DateTime(2025, 7, 12, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7626));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 6,
                column: "TestDate",
                value: new DateTime(2025, 7, 14, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7627));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 7,
                column: "TestDate",
                value: new DateTime(2025, 7, 17, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 8,
                column: "TestDate",
                value: new DateTime(2025, 7, 19, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7630));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 9,
                column: "TestDate",
                value: new DateTime(2025, 7, 20, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7631));

            migrationBuilder.UpdateData(
                table: "TestResults",
                keyColumn: "ResultId",
                keyValue: 10,
                column: "TestDate",
                value: new DateTime(2025, 7, 21, 3, 28, 4, 317, DateTimeKind.Local).AddTicks(7632));
        }
    }
}
