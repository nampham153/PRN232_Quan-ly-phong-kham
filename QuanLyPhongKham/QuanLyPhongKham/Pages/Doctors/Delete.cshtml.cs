using BusinessAccessLayer.IService;
using BusinessAccessLayer.Mappers;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyPhongKham.Pages.Doctors
{
    public class DeleteModel : PageModel
    {
        private readonly IDoctorService _doctorService;

        public DeleteModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
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
            if (Doctor == null || Doctor.AccountId == 0)
            {
                return BadRequest();
            }

            _doctorService.DeleteDoctor(Doctor.AccountId);

            return RedirectToPage("Index");
        }
    }
}
