using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ViewModels
{
    public class PrescriptionViewModel
    {
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hồ sơ bệnh án")]
        public int? RecordId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thuốc")]
        public int? MedicineId { get; set; }

        public string? MedicineName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập liều dùng")]
        public string? Dosage { get; set; }

        public DateTime Date { get; set; } // Thêm thời gian tạo

        public string? Diagnosis { get; set; } // Thêm chẩn đoán từ MedicalRecord

        public string? DoctorName { get; set; } // Thêm tên bác sĩ từ User
    }
}