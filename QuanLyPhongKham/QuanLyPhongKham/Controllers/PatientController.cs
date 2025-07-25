using BusinessAccessLayer.IService;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PatientViewModel>> GetAllPatients()
        {
            var patients = _patientService.GetAllPatients();
            var patientViewModels = patients.Select(p => new PatientViewModel
            {
                PatientId = p.PatientId,
                FullName = p.FullName,
                Gender = p.Gender,
                DOB = (DateTime)p.DOB,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                MedicalRecordCount = p.MedicalRecords?.Count ?? 0,
                AvatarPath = p.AvatarPath,
                UnderlyingDiseases = p.UnderlyingDiseases,
                DiseaseDetails = p.DiseaseDetails
            }).ToList();

            return Ok(patientViewModels);
        }

        [HttpGet("{id}")]
        public ActionResult<PatientViewModel> GetPatientById(int id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            var viewModel = new PatientViewModel
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                Gender = patient.Gender,
                DOB = (DateTime)patient.DOB,
                Phone = patient.Phone,
                Email = patient.Email,
                Address = patient.Address,
                MedicalRecordCount = patient.MedicalRecords?.Count ?? 0,
                AvatarPath = patient.AvatarPath,
                UnderlyingDiseases = patient.UnderlyingDiseases,
                DiseaseDetails = patient.DiseaseDetails
            };

            return Ok(viewModel);
        }

        [HttpPost]
        public ActionResult AddPatient([FromBody] PatientViewModel patientViewModel)
        {
            ModelState.Remove(nameof(patientViewModel.AvatarPath));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = new DataAccessLayer.models.Patient
            {
                FullName = patientViewModel.FullName,
                Gender = patientViewModel.Gender,
                DOB = patientViewModel.DOB,
                Phone = patientViewModel.Phone,
                Email = patientViewModel.Email,
                Address = patientViewModel.Address,
                AvatarPath = patientViewModel.AvatarPath,
                UnderlyingDiseases = patientViewModel.UnderlyingDiseases,
                DiseaseDetails = patientViewModel.DiseaseDetails
            };


            try
            {
                _patientService.AddPatient(patient);
                return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patientViewModel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }


        [HttpPut("{id}")]
        public ActionResult UpdatePatient(int id, [FromBody] PatientViewModel patientViewModel)
        {
            if (id != patientViewModel.PatientId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }

            patient.FullName = patientViewModel.FullName;
            patient.Gender = patientViewModel.Gender;
            patient.DOB = patientViewModel.DOB;
            patient.Phone = patientViewModel.Phone;
            patient.Email = patientViewModel.Email;
            patient.Address = patientViewModel.Address;
            patient.AvatarPath = patientViewModel.AvatarPath;
            patient.UnderlyingDiseases = patientViewModel.UnderlyingDiseases;
            patient.DiseaseDetails = patientViewModel.DiseaseDetails;


            _patientService.UpdatePatient(patient);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePatient(int id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            if (patient.MedicalRecords != null && patient.MedicalRecords.Any())
                return BadRequest("Bệnh nhân có hồ sơ y tế, không thể xóa.");
            _patientService.DeletePatient(id);
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<PatientViewModel>> SearchPatients(
       string fullName = null,
   string phone = null,
   string email = null,
   string address = null,
   string gender = null,
   DateTime? dobFrom = null,
   DateTime? dobTo = null,
   string underlyingDiseases = null,
   string diseaseDetails = null)
        {
            var patients = _patientService.SearchPatients(
                fullName, phone, email, address, gender, dobFrom, dobTo,
                underlyingDiseases, diseaseDetails
            );

            var patientViewModels = patients.Select(p => new PatientViewModel
            {
                PatientId = p.PatientId,
                FullName = p.FullName,
                Gender = p.Gender,
                DOB = (DateTime)p.DOB,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                MedicalRecordCount = p.MedicalRecords?.Count ?? 0,
                AvatarPath = p.AvatarPath,
                UnderlyingDiseases = p.UnderlyingDiseases,
                DiseaseDetails = p.DiseaseDetails
            }).ToList();
            return Ok(patientViewModels);
        }
    }
}