using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    public class PrescriptionViewModel
    {
        public int PrescriptionId { get; set; }
        public int RecordId { get; set; }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; }

        public int Quantity { get; set; }
        public string Dosage { get; set; }

        public string? RecordSummary { get; set; } // ví dụ: ngày khám
    }

}
