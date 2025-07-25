using BusinessAccessLayer.IService;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // Lấy tất cả đơn thuốc
        [HttpGet]
        public ActionResult<IEnumerable<PrescriptionViewModel>> GetAllPrescriptions()
        {
            var list = _prescriptionService.GetAllPrescriptions()
                .Select(p => new PrescriptionViewModel
                {
                    PrescriptionId = p.PrescriptionId,
                    RecordId = p.RecordId,
                    MedicineId = p.MedicineId,
                    MedicineName = p.MedicineName,
                   
                    Dosage = p.Dosage
                }).ToList();

            return Ok(list);
        }

        // Lấy đơn thuốc theo ID
        [HttpGet("{id}")]
        public ActionResult<PrescriptionViewModel> GetPrescriptionById(int id)
        {
            var p = _prescriptionService.GetPrescriptionById(id);
            if (p == null) return NotFound();

            return Ok(new PrescriptionViewModel
            {
                PrescriptionId = p.PrescriptionId,
                RecordId = p.RecordId,
                MedicineId = p.MedicineId,
                MedicineName = p.MedicineName,
                
                Dosage = p.Dosage
            });
        }

        // Thêm mới đơn thuốc
        [HttpPost]
        public ActionResult AddPrescription([FromBody] PrescriptionViewModel vm)
        {
            if (!ModelState.IsValid || vm.RecordId == null || vm.MedicineId == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var prescription = new Prescription
            {
                RecordId = vm.RecordId.Value,
                MedicineId = vm.MedicineId.Value,
               
                Dosage = vm.Dosage ?? string.Empty
            };

            var created = _prescriptionService.CreatePrescription(prescription);

            return CreatedAtAction(nameof(GetPrescriptionById), new { id = created.PrescriptionId }, vm);
        }

        // Cập nhật đơn thuốc
        [HttpPut("{id}")]
        public ActionResult UpdatePrescription(int id, [FromBody] PrescriptionViewModel vm)
        {
            if (id != vm.PrescriptionId || !ModelState.IsValid || vm.RecordId == null || vm.MedicineId == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var prescription = _prescriptionService.GetPrescriptionEntityById(id);
            if (prescription == null) return NotFound();

            prescription.RecordId = vm.RecordId ?? 0;
            prescription.MedicineId = vm.MedicineId ?? 0;
           
            prescription.Dosage = vm.Dosage ?? string.Empty;

            _prescriptionService.UpdatePrescription(prescription);
            return NoContent();
        }

        // Xóa đơn thuốc
        [HttpDelete("{id}")]
        public ActionResult DeletePrescription(int id)
        {
            var prescription = _prescriptionService.GetPrescriptionById(id);
            if (prescription == null) return NotFound();

            _prescriptionService.DeletePrescription(id);
            return NoContent();
        }

        // Tìm kiếm đơn thuốc có phân trang
        [HttpGet("search")]
        public IActionResult SearchPrescriptions(
            int? recordId = null,
            int? medicineId = null,
            
            string? dosage = null,
            int page = 1,
            int pageSize = 5)
        {
            var result = _prescriptionService.SearchPrescriptions(recordId, medicineId,dosage);

            var total = result.Count();
            var data = result.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(p => new PrescriptionViewModel
                {
                    PrescriptionId = p.PrescriptionId,
                    RecordId = p.RecordId,
                    MedicineId = p.MedicineId,
                    MedicineName = p.MedicineName,
                    Dosage = p.Dosage
                }).ToList();

            return Ok(new
            {
                Data = data,
                TotalRecords = total,
                Page = page,
                PageSize = pageSize
            });
        }
    }
}
