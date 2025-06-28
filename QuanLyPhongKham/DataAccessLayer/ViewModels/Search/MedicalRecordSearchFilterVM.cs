using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels.Search
{
    public class MedicalRecordSearchFilterVM
    {
        public string? SearchTerm { get; set; }
        public int? UserId { get; set; }
        public int? PatientId { get; set; }
        public string SortBy { get; set; } = "RecordId";
        public bool SortDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
