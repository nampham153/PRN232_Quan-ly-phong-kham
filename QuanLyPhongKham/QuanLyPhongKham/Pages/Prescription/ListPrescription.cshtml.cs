using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer.ViewModels;
using BusinessAccessLayer.IService;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class ListPrescriptionModel : PageModel
    {
        private readonly IPrescriptionService _prescriptionService;
        private readonly IMedicineService _medicineService;
        private readonly IMedicalRecordService _recordService;

        public ListPrescriptionModel(IPrescriptionService prescriptionService,
                                     IMedicineService medicineService,
                                     IMedicalRecordService recordService)
        {
            _prescriptionService = prescriptionService;
            _medicineService = medicineService;
            _recordService = recordService;
        }

        public List<PrescriptionViewModel> Prescriptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? RecordId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MedicineId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Dosage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Quantity { get; set; }

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

        public void OnGet()
        {
            // Lấy danh sách đơn thuốc sau khi lọc
            Prescriptions = _prescriptionService.SearchPrescriptions(RecordId, MedicineId, Quantity, Dosage);

            // Load danh sách thuốc
            Medicines = new SelectList(_medicineService.GetAllMedicines(), "MedicineId", "MedicineName");

            // Load danh sách hồ sơ
            Records = new SelectList(_recordService.GetAll(), "RecordId", "RecordId"); // hoặc có thể dùng PatientName
        }
    }
}
