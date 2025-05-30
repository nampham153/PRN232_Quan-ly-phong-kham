using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.models
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Unit { get; set; }
        public string Usage { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
