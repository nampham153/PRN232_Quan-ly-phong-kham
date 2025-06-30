using BusinessAccessLayer.IService;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]

    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PrescriptionViewModel>> GetAllPrescriptions()
        {
            var prescriptionViewModels = _prescriptionService.GetAllPrescriptions(); 
            return Ok(prescriptionViewModels);
        }


        [HttpGet("{id}")]
        public ActionResult<PrescriptionViewModel> GetPrescriptionById(int id)
        {
            var prescription = _prescriptionService.GetPrescriptionById(id);
            if (prescription == null)
            {
                return NotFound();
            }
            var viewModel = new PrescriptionViewModel
            {
                PrescriptionId = prescription.PrescriptionId,
                RecordId = prescription.RecordId,
                MedicineId = prescription.MedicineId,
                MedicineName = prescription.MedicineName,
                Quantity = prescription.Quantity,
                Dosage = prescription.Dosage
            };
            return Ok(viewModel);
        }

        [HttpPost]
        public ActionResult AddPrescription([FromBody] PrescriptionViewModel prescriptionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prescription = new Prescription
            {
                RecordId = prescriptionViewModel.RecordId,
                MedicineId = prescriptionViewModel.MedicineId,
                Quantity = prescriptionViewModel.Quantity,
                Dosage = prescriptionViewModel.Dosage
            };

            var createdPrescription = _prescriptionService.CreatePrescription(prescription);
            return CreatedAtAction(nameof(GetPrescriptionById), new { id = createdPrescription.PrescriptionId }, prescriptionViewModel);
        }

        [HttpPut("{id}")]
        public ActionResult UpdatePrescription(int id, [FromBody] PrescriptionViewModel prescriptionViewModel)
        {
            if (id != prescriptionViewModel.PrescriptionId || !ModelState.IsValid)
                return BadRequest();

            var prescription = _prescriptionService.GetPrescriptionEntityById(id);
            if (prescription == null)
                return NotFound();

            // Gán lại các giá trị
            prescription.RecordId = prescriptionViewModel.RecordId;
            prescription.MedicineId = prescriptionViewModel.MedicineId;
            prescription.Quantity = prescriptionViewModel.Quantity;
            prescription.Dosage = prescriptionViewModel.Dosage;

            _prescriptionService.UpdatePrescription(prescription);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult DeletePrescription(int id)
        {
            var prescription = _prescriptionService.GetPrescriptionById(id);
            if (prescription == null)
            {
                return NotFound();
            }
            _prescriptionService.DeletePrescription(id);
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<PrescriptionViewModel>> SearchPrescriptions(
            int? recordId = null,
            int? medicineId = null,
            int? quantity = null,
            string dosage = null)
        {
            var prescriptions = _prescriptionService.SearchPrescriptions(recordId, medicineId, quantity, dosage);
            var prescriptionViewModels = prescriptions.Select(p => new PrescriptionViewModel
            {
                PrescriptionId = p.PrescriptionId,
                RecordId = p.RecordId,
                MedicineId = p.MedicineId,
                MedicineName = p.MedicineName,
                Quantity = p.Quantity,
                Dosage = p.Dosage
            }).ToList();
            return Ok(prescriptionViewModels);
        }
    }
}