using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    public class PrescriptionVM
    {
        public int PrescriptionId { get; set; }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; }

        public int Quantity { get; set; }
        public string Dosage { get; set; }
        public string Unit { get; set; }
        public string Usage { get; set; }
    }

}
