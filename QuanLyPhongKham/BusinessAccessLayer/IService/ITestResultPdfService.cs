using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using DataAccessLayer.ViewModels;
using DataAccessLayer.models;
using iTextSharp.text.pdf.draw;

// Add to Program.cs:
// builder.Services.AddScoped<ITestResultPdfService, TestResultPdfService>();

namespace BusinessAccessLayer.Service
{
    public interface ITestResultPdfService
    {
        byte[] GenerateTestResultPdf(TestResultVM testResult);
    }

    public class TestResultPdfService : ITestResultPdfService
    {
        public byte[] GenerateTestResultPdf(TestResultVM testResult)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Create PDF document with proper margins
                var document = new Document(PageSize.A4, 50, 50, 70, 70);
                var writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                // Define professional fonts with proper encoding
                BaseFont timesRoman = BaseFont.CreateFont("c:/windows/fonts/times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont timesBold = BaseFont.CreateFont("c:/windows/fonts/timesbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont arial = BaseFont.CreateFont("c:/windows/fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont arialBold = BaseFont.CreateFont("c:/windows/fonts/arialbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Fallback to built-in fonts if system fonts are not available
                BaseFont baseFont = arial ?? BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont baseFontBold = arialBold ?? BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                // Define professional font styles
                Font titleFont = new Font(baseFontBold, 20f, Font.BOLD, new BaseColor(41, 74, 122)); // Professional blue
                Font headerFont = new Font(baseFontBold, 16f, Font.BOLD, BaseColor.Black);
                Font subHeaderFont = new Font(baseFontBold, 13f, Font.BOLD, new BaseColor(60, 60, 60));
                Font normalFont = new Font(baseFont, 11f, Font.NORMAL, BaseColor.Black);
                Font normalBoldFont = new Font(baseFontBold, 11f, Font.BOLD, BaseColor.Black);
                Font smallFont = new Font(baseFont, 9f, Font.NORMAL, new BaseColor(100, 100, 100));
                Font labelFont = new Font(baseFontBold, 10f, Font.BOLD, new BaseColor(80, 80, 80));

                // Hospital/Clinic Header
                var hospitalTable = new PdfPTable(2);
                hospitalTable.WidthPercentage = 100;
                hospitalTable.SetWidths(new float[] { 2f, 1f });
                hospitalTable.SpacingAfter = 15f;

                // Logo and hospital information (left side)
                var hospitalInfoCell = new PdfPCell();
                hospitalInfoCell.Border = Rectangle.NO_BORDER;
                hospitalInfoCell.VerticalAlignment = Element.ALIGN_TOP;

                var clinicTitle = new Paragraph("GENERAL MEDICAL CLINIC", headerFont);
                clinicTitle.SpacingAfter = 5f;
                hospitalInfoCell.AddElement(clinicTitle);

                hospitalInfoCell.AddElement(new Paragraph("ABC MEDICAL CENTER", normalBoldFont));
                hospitalInfoCell.AddElement(new Paragraph("123 ABC Street, XYZ District, Ho Chi Minh City", normalFont));
                hospitalInfoCell.AddElement(new Paragraph("Phone: (028) 1234-5678", normalFont));
                hospitalInfoCell.AddElement(new Paragraph("Email: info@abcmedical.com", normalFont));
                hospitalInfoCell.AddElement(new Paragraph("Website: www.abcmedical.com", normalFont));

                hospitalTable.AddCell(hospitalInfoCell);

                // Print date information (right side)
                var dateInfoCell = new PdfPCell();
                dateInfoCell.Border = Rectangle.NO_BORDER;
                dateInfoCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                dateInfoCell.VerticalAlignment = Element.ALIGN_TOP;

                dateInfoCell.AddElement(new Paragraph($"Print Date: {DateTime.Now:MM/dd/yyyy}", labelFont));
                dateInfoCell.AddElement(new Paragraph($"Print Time: {DateTime.Now:HH:mm}", labelFont));
                dateInfoCell.AddElement(new Paragraph($"Report ID: TR-{DateTime.Now:yyyyMMdd}-{testResult.RecordId}", smallFont));

                hospitalTable.AddCell(dateInfoCell);

                document.Add(hospitalTable);

                // Decorative line
                var line = new Paragraph();
                line.Add(new Chunk(new LineSeparator(2f, 100f, new BaseColor(41, 74, 122), Element.ALIGN_CENTER, -1)));
                document.Add(line);
                document.Add(new Paragraph(" "));

                // Main title
                var title = new Paragraph("LABORATORY TEST RESULTS", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 25f;
                document.Add(title);

                // Patient and test information
                var infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 1f, 1f });
                infoTable.SpacingAfter = 20f;

                // Left column - patient information
                var leftPatientCell = new PdfPCell();
                leftPatientCell.Border = Rectangle.BOX;
                leftPatientCell.BorderColor = new BaseColor(200, 200, 200);
                leftPatientCell.BorderWidth = 1f;
                leftPatientCell.Padding = 12f;
                leftPatientCell.BackgroundColor = new BaseColor(248, 249, 250);

                var patientTitle = new Paragraph("PATIENT INFORMATION", subHeaderFont);
                patientTitle.SpacingAfter = 8f;
                leftPatientCell.AddElement(patientTitle);

                var patientInfo = new Paragraph();
                patientInfo.Add(new Chunk("Full Name: ", labelFont));
                patientInfo.Add(new Chunk(testResult.PatientName ?? "N/A", normalFont));
                patientInfo.SpacingAfter = 4f;
                leftPatientCell.AddElement(patientInfo);

                var examDate = new Paragraph();
                examDate.Add(new Chunk("Examination Date: ", labelFont));
                examDate.Add(new Chunk(testResult.MedicalRecordDate ?? "N/A", normalFont));
                examDate.SpacingAfter = 4f;
                leftPatientCell.AddElement(examDate);

                var diagnosis = new Paragraph();
                diagnosis.Add(new Chunk("Diagnosis: ", labelFont));
                diagnosis.Add(new Chunk(testResult.Diagnosis ?? "N/A", normalFont));
                leftPatientCell.AddElement(diagnosis);

                infoTable.AddCell(leftPatientCell);

                // Right column - test information
                var rightTestCell = new PdfPCell();
                rightTestCell.Border = Rectangle.BOX;
                rightTestCell.BorderColor = new BaseColor(200, 200, 200);
                rightTestCell.BorderWidth = 1f;
                rightTestCell.Padding = 12f;
                rightTestCell.BackgroundColor = new BaseColor(248, 249, 250);

                var testTitle = new Paragraph("TEST INFORMATION", subHeaderFont);
                testTitle.SpacingAfter = 8f;
                rightTestCell.AddElement(testTitle);

                var testName = new Paragraph();
                testName.Add(new Chunk("Test Name: ", labelFont));
                testName.Add(new Chunk(testResult.TestName ?? "N/A", normalFont));
                testName.SpacingAfter = 4f;
                rightTestCell.AddElement(testName);

                var testDate = new Paragraph();
                testDate.Add(new Chunk("Test Date: ", labelFont));
                testDate.Add(new Chunk(testResult.TestDate.ToString("MM/dd/yyyy HH:mm"), normalFont));
                testDate.SpacingAfter = 4f;
                rightTestCell.AddElement(testDate);

                var technician = new Paragraph();
                technician.Add(new Chunk("Laboratory Technician: ", labelFont));
                technician.Add(new Chunk(testResult.UserName ?? "N/A", normalFont));
                rightTestCell.AddElement(technician);

                infoTable.AddCell(rightTestCell);

                document.Add(infoTable);

                // Test description (if available)
                if (!string.IsNullOrEmpty(testResult.TestDescription))
                {
                    var descriptionParagraph = new Paragraph();
                    descriptionParagraph.Add(new Chunk("Test Description: ", labelFont));
                    descriptionParagraph.Add(new Chunk(testResult.TestDescription, normalFont));
                    descriptionParagraph.SpacingAfter = 20f;
                    document.Add(descriptionParagraph);
                }

                // Test results section
                var resultTitle = new Paragraph("LABORATORY RESULTS", headerFont);
                resultTitle.SpacingAfter = 15f;
                document.Add(resultTitle);

                // Results table
                var resultTable = new PdfPTable(1);
                resultTable.WidthPercentage = 100;
                resultTable.SpacingAfter = 25f;

                var resultCell = new PdfPCell();
                resultCell.Border = Rectangle.BOX;
                resultCell.BorderColor = new BaseColor(200, 200, 200);
                resultCell.BorderWidth = 1f;
                resultCell.Padding = 20f;
                resultCell.MinimumHeight = 120f;
                resultCell.BackgroundColor = BaseColor.White;

                var resultContent = testResult.ResultDetail ?? "Results pending";
                var resultParagraph = new Paragraph(resultContent, normalFont);
                resultParagraph.Leading = 18f; // Line spacing
                resultParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                resultCell.AddElement(resultParagraph);

                resultTable.AddCell(resultCell);
                document.Add(resultTable);

                // Notes and signature section
                var signatureTable = new PdfPTable(2);
                signatureTable.WidthPercentage = 100;
                signatureTable.SetWidths(new float[] { 1.2f, 0.8f });
                signatureTable.SpacingBefore = 30f;

                // Notes (left side)
                var noteCell = new PdfPCell();
                noteCell.Border = Rectangle.NO_BORDER;
                noteCell.VerticalAlignment = Element.ALIGN_TOP;

                var notesTitle = new Paragraph("IMPORTANT NOTES:", labelFont);
                notesTitle.SpacingAfter = 8f;
                noteCell.AddElement(notesTitle);

                noteCell.AddElement(new Paragraph("• This result is valid only with authorized signature", smallFont));
                noteCell.AddElement(new Paragraph("• Contact the clinic if you have any questions", smallFont));
                noteCell.AddElement(new Paragraph("• Keep this report in a safe place", smallFont));
                noteCell.AddElement(new Paragraph("• Follow up with your physician as recommended", smallFont));

                signatureTable.AddCell(noteCell);

                // Signature (right side)
                var signatureCell = new PdfPCell();
                signatureCell.Border = Rectangle.NO_BORDER;
                signatureCell.HorizontalAlignment = Element.ALIGN_CENTER;
                signatureCell.VerticalAlignment = Element.ALIGN_TOP;

                var location = new Paragraph($"Ho Chi Minh City, {DateTime.Now:MMMM dd, yyyy}", normalFont);
                location.Alignment = Element.ALIGN_CENTER;
                location.SpacingAfter = 10f;
                signatureCell.AddElement(location);

                var signatureTitle = new Paragraph("LABORATORY TECHNICIAN", labelFont);
                signatureTitle.Alignment = Element.ALIGN_CENTER;
                signatureTitle.SpacingAfter = 40f;
                signatureCell.AddElement(signatureTitle);

                var technicianName = new Paragraph(testResult.UserName ?? "___________________", normalFont);
                technicianName.Alignment = Element.ALIGN_CENTER;
                signatureCell.AddElement(technicianName);

                signatureTable.AddCell(signatureCell);

                document.Add(signatureTable);

                // Footer
                document.Add(new Paragraph(" "));
                var footerLine = new Paragraph();
                footerLine.Add(new Chunk(new LineSeparator(1f, 100f, new BaseColor(200, 200, 200), Element.ALIGN_CENTER, -1)));
                document.Add(footerLine);

                var footer = new Paragraph("This report was generated automatically by the clinic management system", smallFont);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 8f;
                document.Add(footer);

                document.Close();
                writer.Close();

                return memoryStream.ToArray();
            }
        }
    }
}