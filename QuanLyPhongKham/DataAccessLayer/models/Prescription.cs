using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        public int RecordId { get; set; }
        [ForeignKey("RecordId")]
        public MedicalRecord MedicalRecord { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public string Dosage { get; set; }
        public int Quantity { get; set; }

        public DateTime Date { get; set; } // Thêm thời gian tạo đơn thuốc
    }
}