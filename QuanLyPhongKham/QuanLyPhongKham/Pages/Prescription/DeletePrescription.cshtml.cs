using BusinessAccessLayer.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class DeletePrescriptionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;

        public DeletePrescriptionModel(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [BindProperty]
        public DataAccessLayer.models.Prescription Prescription { get; set; }

        public IActionResult OnGet(int id)
        {
            var entity = _prescriptionService.GetPrescriptionEntityById(id);
            if (entity == null)
            {
                return NotFound();
            }

            Prescription = entity;
            return Page();
        }

        public IActionResult OnPost()
        {
            _prescriptionService.DeletePrescription(Prescription.PrescriptionId);
            return RedirectToPage("ListPrescription");
        }
    }

}
