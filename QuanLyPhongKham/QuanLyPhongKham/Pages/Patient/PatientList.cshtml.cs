using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyPhongKham.Pages.Patient
{
    public class PatientListModel : PageModel
    {
        private readonly IPatientService _patientService;

        public PatientListModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public List<PatientViewModel> Patients { get; set; }

        public void OnGet()
        {
            var patients = _patientService.GetAllPatients();
            Patients = patients.Select(p => new PatientViewModel
            {
                PatientId = p.PatientId,
                FullName = p.FullName,
                Gender = p.Gender,
                DOB = (DateTime)p.DOB,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                MedicalRecordCount = p.MedicalRecords?.Count ?? 0
            }).ToList();
        }
    }
}
