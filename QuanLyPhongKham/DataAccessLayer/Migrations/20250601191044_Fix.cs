 using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Users_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_MedicalRecords_MedicalRecordRecordId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_MedicalRecords_MedicalRecordRecordId",
                table: "TestResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Users_TechnicianId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_MedicalRecordRecordId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_MedicalRecordRecordId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "MedicalRecordRecordId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "MedicalRecordRecordId",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "TechnicianId",
                table: "TestResults",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TestResults_TechnicianId",
                table: "TestResults",
                newName: "IX_TestResults_UserId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "MedicalRecords",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_DoctorId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_RecordId",
                table: "TestResults",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_RecordId",
                table: "Prescriptions",
                column: "RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Users_UserId",
                table: "MedicalRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_MedicalRecords_RecordId",
                table: "Prescriptions",
                column: "RecordId",
                principalTable: "MedicalRecords",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_MedicalRecords_RecordId",
                table: "TestResults",
                column: "RecordId",
                principalTable: "MedicalRecords",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Users_UserId",
                table: "TestResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Users_UserId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_MedicalRecords_RecordId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_MedicalRecords_RecordId",
                table: "TestResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Users_UserId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_RecordId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_RecordId",
                table: "Prescriptions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TestResults",
                newName: "TechnicianId");

            migrationBuilder.RenameIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults",
                newName: "IX_TestResults_TechnicianId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MedicalRecords",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_UserId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_DoctorId");

            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordRecordId",
                table: "TestResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MedicalRecordRecordId",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_MedicalRecordRecordId",
                table: "TestResults",
                column: "MedicalRecordRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_MedicalRecordRecordId",
                table: "Prescriptions",
                column: "MedicalRecordRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Users_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_MedicalRecords_MedicalRecordRecordId",
                table: "Prescriptions",
                column: "MedicalRecordRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_MedicalRecords_MedicalRecordRecordId",
                table: "TestResults",
                column: "MedicalRecordRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "RecordId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Users_TechnicianId",
                table: "TestResults",
                column: "TechnicianId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
