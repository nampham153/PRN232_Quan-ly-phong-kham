using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using DataAccessLayer.models;
using DataAccessLayer.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessAccessLayer.Mappers;

namespace QuanLyPhongKham.Pages.Doctors
{
    public class EditModel : PageModel
    {
        private readonly IDoctorService _doctorService;
        private readonly IAccountRepository _accountRepository;

        public EditModel(IDoctorService doctorService, IAccountRepository accountRepository)
        {
            _doctorService = doctorService;
            _accountRepository = accountRepository;
        }

        [BindProperty]
        public DoctorVM Doctor { get; set; }

        public IActionResult OnGet(int id)
        {
            var doctorEntity = _doctorService.GetDoctorByAccountId(id);
            if (doctorEntity == null)
            {
                return NotFound();
            }
            Doctor = DoctorMapper.ToViewModel(doctorEntity);

            return Page();
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

            var updatedDoctor = new User
            {
                AccountId = Doctor.AccountId,
                FullName = Doctor.FullName,
                Email = Doctor.Email,
                Phone = Doctor.Phone,
                DOB = Doctor.DOB,
                Gender = Doctor.Gender
            };

            _doctorService.UpdateDoctor(Doctor.AccountId, updatedDoctor);

            return RedirectToPage("Index");
        }
    }
}
