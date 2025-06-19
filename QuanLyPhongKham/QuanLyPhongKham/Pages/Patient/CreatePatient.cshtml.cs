using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyPhongKham.Pages.Patient
{
    public class CreatePatientModel : PageModel
    {
        private readonly IPatientService _patientService;

        public CreatePatientModel(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [BindProperty]
        public PatientViewModel PatientViewModel { get; set; }

        [BindProperty]
        public IFormFile AvatarFile { get; set; } // Sử dụng để upload file

        public IActionResult OnGet()
        {
            PatientViewModel = new PatientViewModel();
            return Page();
        }

        public IActionResult OnPost()
        {
            // Nếu không upload file và không có link -> báo lỗi
            if ((AvatarFile == null || AvatarFile.Length == 0) && string.IsNullOrWhiteSpace(PatientViewModel.AvatarPath))
            {
                ModelState.AddModelError("AvatarFile", "Bạn phải chọn ảnh từ máy hoặc dán link ảnh.");
            }

            // Nếu có upload file thì bỏ qua bắt buộc của AvatarPath (coi như không cần nhập)
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                ModelState.Remove("PatientViewModel.AvatarPath");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var patient = new DataAccessLayer.models.Patient
                {
                    FullName = PatientViewModel.FullName,
                    Gender = PatientViewModel.Gender,
                    DOB = PatientViewModel.DOB,
                    Phone = PatientViewModel.Phone,
                    Email = PatientViewModel.Email,
                    Address = PatientViewModel.Address
                };

                // Ưu tiên file upload
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot/uploadsPatient");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AvatarFile.CopyTo(stream);
                    }

                    patient.AvatarPath = $"/uploadsPatient/{fileName}";
                }
                else if (!string.IsNullOrWhiteSpace(PatientViewModel.AvatarPath))
                {
                    patient.AvatarPath = PatientViewModel.AvatarPath.Trim();
                }

                _patientService.AddPatient(patient);
                return RedirectToPage("/Patient/PatientList");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi thêm bệnh nhân: " + ex.Message);
                return Page();
            }
        }

    }
}