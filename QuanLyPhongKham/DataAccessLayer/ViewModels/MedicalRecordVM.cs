using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer.models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DataAccessLayer.ViewModels
{
    public class MedicalRecordVM
    {
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bệnh nhân.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn bệnh nhân hợp lệ.")]
        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bác sĩ.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn bác sĩ hợp lệ.")]
        public int UserId { get; set; }
        public string? DoctorName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public int Status { get; set; } = 1;

        [Required(ErrorMessage = "Vui lòng nhập triệu chứng.")]
        public string Symptoms { get; set; }
        public string Diagnosis { get; set; }

        public string Note { get; set; }

        public List<TestSummaryVM> TestSummaries { get; set; } = new();
    }
}

