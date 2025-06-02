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

        [BindProperty(SupportsGet = true)]
        public string SearchFullName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchPhone { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEmail { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchAddress { get; set; }

        [BindProperty(SupportsGet = true)]
        public string GenderFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DOBFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DOBTo { get; set; }

        public void OnGet()
        {
            var patients = _patientService.SearchPatients(SearchFullName, SearchPhone, SearchEmail, SearchAddress, GenderFilter, DOBFrom, DOBTo);
            Patients = patients.Select(p => new PatientViewModel
            {
                PatientId = p.PatientId,
                FullName = p.FullName,
                Gender = p.Gender,
                DOB = (DateTime)p.DOB, // Đảm bảo DOB là DateTime, nếu nullable thì xử lý null
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                MedicalRecordCount = p.MedicalRecords?.Count ?? 0
            }).ToList();
        }
    }
}