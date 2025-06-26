using BusinessAccessLayer.IService;
using DataAccessLayer.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class UpdatePrescriptionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicineService _medicineService;
        private readonly IMedicalRecordService _recordService;

        public UpdatePrescriptionModel(IPrescriptionService prescriptionService, IMedicineService medicineService, IMedicalRecordService recordService)
        {
            _prescriptionService = prescriptionService;
            _medicineService = medicineService;
            _recordService = recordService;
        }

        [BindProperty]
        public DataAccessLayer.models.Prescription Prescription { get; set; }

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

        public IActionResult OnGet(int id)
        {
            var entity = _prescriptionService.GetPrescriptionEntityById(id);
            if (entity == null)
            {
                return NotFound();
            }

            Prescription = entity;
            Medicines = new SelectList(_medicineService.GetAllMedicines(), "MedicineId", "MedicineName");
            Records = new SelectList(_recordService.GetAll(), "RecordId", "RecordId");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet(Prescription.PrescriptionId);
                return Page();
            }

            _prescriptionService.UpdatePrescription(Prescription);
            return RedirectToPage("ListPrescription");
        }
    }

}
