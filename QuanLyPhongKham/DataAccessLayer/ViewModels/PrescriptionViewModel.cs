using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    using System.ComponentModel.DataAnnotations;

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

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        public string? RecordSummary { get; set; }
    }


}
