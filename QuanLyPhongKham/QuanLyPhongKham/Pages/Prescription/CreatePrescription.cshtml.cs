using BusinessAccessLayer.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class CreatePrescriptionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicineService _medicineService;
        private readonly IMedicalRecordService _recordService;

        public CreatePrescriptionModel(IPrescriptionService prescriptionService, IMedicineService medicineService, IMedicalRecordService recordService)
        {
            _prescriptionService = prescriptionService;
            _medicineService = medicineService;
            _recordService = recordService;
        }

        [BindProperty]
        public DataAccessLayer.models.Prescription Prescription { get; set; }
        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

        public void OnGet()
        {
            Medicines = new SelectList(_medicineService.GetAllMedicines(), "MedicineId", "MedicineName");
            Records = new SelectList(_recordService.GetAll(), "RecordId", "RecordId");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            _prescriptionService.CreatePrescription(Prescription);
            return RedirectToPage("ListPrescription");
        }
    }

}
