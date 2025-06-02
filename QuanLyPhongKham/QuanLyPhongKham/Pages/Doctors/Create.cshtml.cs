using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using DataAccessLayer.models;
using DataAccessLayer.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public void OnGet()
        {
            Doctor = new DoctorVM();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            var doctorEntity = new User
            {
                FullName = Doctor.FullName,
                Gender = Doctor.Gender,
                DOB = Doctor.DOB,
                Phone = Doctor.Phone,
                Email = Doctor.Email,
                AccountId = Doctor.AccountId
            };

            _doctorService.CreateDoctor(doctorEntity);

            return RedirectToPage("Index");
        }

    }
}
