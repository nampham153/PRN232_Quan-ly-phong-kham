using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using DataAccessLayer.models;
using DataAccessLayer.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace QuanLyPhongKham.Pages.Doctors
{
    public class CreateModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly IAccountRepository _accountRepository;

        public CreateModel(IDoctorService doctorService, IAccountRepository accountRepository)
        {
            _doctorService = doctorService;
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public DoctorVM Doctor { get; set; }
        [BindProperty]
        public IFormFile DoctorFile { get; set; }

        public List<Account> DoctorAccounts { get; set; }

        public void OnGet()
        {
            Doctor = new DoctorVM();
            DoctorAccounts = _accountRepository.GetAvailableAccountsForDoctor();
        }

        public IActionResult OnPost()
        {
            DoctorAccounts = _accountRepository.GetDoctorAccounts();
            var account = _accountRepository.GetAccountById(Doctor.AccountId);
            if (account == null || account.RoleId != 2)
            {
                ModelState.AddModelError("Doctor.AccountId", "Tài khoản không tồn tại hoặc không phải tài khoản bác sĩ.");
                return Page();
            }

            var existingUser = _doctorService.GetDoctorByAccountId(Doctor.AccountId);
            if (existingUser != null)
            {
                ModelState.AddModelError("Doctor.AccountId", "Tài khoản này đã được đăng ký.");
                return Page();
            }
            if (!Doctor.DOB.HasValue)
            {
                ModelState.AddModelError("Doctor.DOB", "Vui lòng nhập ngày sinh.");
                return Page();
            }

            var today = DateTime.Today;
            var dob = Doctor.DOB.Value;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;

            if (age < 18)
            {
                ModelState.AddModelError("Doctor.DOB", "Bác sĩ phải từ 18 tuổi trở lên.");
                return Page();
            }
            if ((DoctorFile == null || DoctorFile.Length == 0) && string.IsNullOrWhiteSpace(Doctor.DoctorPath))
            {
                ModelState.AddModelError("AvatarFile", "Bạn phải chọn ảnh từ máy hoặc dán link ảnh.");
            }

            if (DoctorFile == null || DoctorFile.Length == 0)
            {
                ModelState.Remove(nameof(DoctorFile)); 
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var doctorEntity = new User
            {
                FullName = Doctor.FullName,
                Gender = Doctor.Gender,
                DOB = Doctor.DOB,
                Phone = Doctor.Phone,
                Email = Doctor.Email,
                AccountId = Doctor.AccountId
            };

                if (DoctorFile != null && DoctorFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine("wwwroot/uploadsDoctor");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid() + Path.GetExtension(DoctorFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        DoctorFile.CopyTo(stream);
                    }

                    doctorEntity.DoctorPath = $"/uploadsDoctor/{fileName}";
                }
                else
                {
                    doctorEntity.DoctorPath = Doctor.DoctorPath?.Trim();
                }


                _doctorService.CreateDoctor(doctorEntity);
            return RedirectToPage("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi thêm bac si: " + ex.Message);
                return Page();
            }
        }
    }
}
