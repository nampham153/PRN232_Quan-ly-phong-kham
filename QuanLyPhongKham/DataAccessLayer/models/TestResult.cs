using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.models
{
    public class TestResult
    {
        [Key]
        public int ResultId { get; set; }

        public int RecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public int TechnicianId { get; set; }
        public User Technician { get; set; }

        public string ResultDetail { get; set; }
        public DateTime TestDate { get; set; }
    }
}