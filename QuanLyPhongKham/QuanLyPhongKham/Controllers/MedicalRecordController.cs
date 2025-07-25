 using BusinessAccessLayer.IService;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] MedicalRecordVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_medicalRecordService.PatientHasRecord(model.PatientId))
            {
                return BadRequest(new { message = "Bệnh nhân đã có hồ sơ y tế." });
            }

            var newRecord = new MedicalRecord
            {
                PatientId = model.PatientId,
                UserId = model.UserId,
                Date = model.Date,
                Symptoms = model.Symptoms,
                Diagnosis = model.Diagnosis,
                Note = model.Note
            };

            try
            {
                _medicalRecordService.Add(newRecord);
                return Ok(new { success = true, recordId = newRecord.RecordId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MedicalRecordVM model)
        {
            var existing = _medicalRecordService.GetById(id);
            if (existing == null) return NotFound();

            existing.PatientId = model.PatientId;
            existing.UserId = model.UserId;
            existing.Date = model.Date;
            existing.Symptoms = model.Symptoms;
            existing.Diagnosis = model.Diagnosis;
            existing.Note = model.Note;

            _medicalRecordService.Update(existing);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var record = _medicalRecordService.GetById(id);
            if (record == null) return NotFound();

            _medicalRecordService.Delete(record);
            return Ok();
        }

        // GET: api/MedicalRecord/filter?searchTerm=&userId=&patientId=&sortBy=&sortDescending=&page=&pageSize=
        [HttpGet("filter")]
        public IActionResult Filter(
    [FromQuery] string? searchTerm,
    [FromQuery] int? userId,
    [FromQuery] int? patientId,
    [FromQuery] string sortBy = "RecordId",
    [FromQuery] bool sortDescending = false,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var query = _medicalRecordService.QueryAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var term = searchTerm.ToLower();
                query = query.Where(r =>
                    (r.Patient != null && r.Patient.FullName.ToLower().Contains(term)) ||
                    (r.User != null && r.User.FullName.ToLower().Contains(term)) ||
                    (r.Symptoms != null && r.Symptoms.ToLower().Contains(term)));
            }

            if (userId.HasValue)
                query = query.Where(r => r.UserId == userId.Value);

            if (patientId.HasValue)
                query = query.Where(r => r.PatientId == patientId.Value);

            if (sortBy == "Date")
                query = sortDescending ? query.OrderByDescending(r => r.Date) : query.OrderBy(r => r.Date);
            else if (sortBy == "User.FullName")
                query = sortDescending
                    ? query.OrderByDescending(r => r.User != null ? r.User.FullName : "")
                    : query.OrderBy(r => r.User != null ? r.User.FullName : "");
            else if (sortBy == "Patient.FullName")
                query = sortDescending
                    ? query.OrderByDescending(r => r.Patient != null ? r.Patient.FullName : "")
                    : query.OrderBy(r => r.Patient != null ? r.Patient.FullName : "");
            else
                query = sortDescending ? query.OrderByDescending(r => r.RecordId) : query.OrderBy(r => r.RecordId);

            var totalRecords = query.Count();

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new MedicalRecordVM
                {
                    RecordId = r.RecordId,
                    PatientId = r.PatientId,
                    PatientName = r.Patient != null ? r.Patient.FullName : null,
                    UserId = r.UserId,
                    DoctorName = r.User != null ? r.User.FullName : null,
                    Date = r.Date,
                    Symptoms = r.Symptoms,
                    Diagnosis = r.Diagnosis,
                    Note = r.Note
                })
                .ToList();

            return Ok(new
            {
                Data = data,
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize
            });
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var record = _medicalRecordService.GetById(id);
            if (record == null)
                return NotFound();

            var recordVm = new MedicalRecordVM
            {
                RecordId = record.RecordId,
                PatientId = record.PatientId,
                PatientName = record.Patient?.FullName,
                UserId = record.UserId,
                DoctorName = record.User?.FullName,
                Date = record.Date,
                Symptoms = record.Symptoms,
                Diagnosis = record.Diagnosis,
                Note = record.Note,
                TestSummaries = record.TestResults
                    .GroupBy(tr => tr.TestDate.Date)
                    .OrderByDescending(g => g.Key)
                    .Select(g => new TestSummaryVM
                    {
                        TestDate = g.Key,
                        Results = g.Select(tr => new TestResultVM
                        {
                            RecordId = tr.RecordId,
                            TestId = tr.TestId,
                            UserId = tr.UserId,
                            ResultDetail = tr.ResultDetail,
                            TestDate = tr.TestDate,
                            TestName = tr.Test?.TestName,
                            TestDescription = tr.Test?.Description,
                            UserName = tr.User?.FullName,
                            PatientName = record.Patient?.FullName,
                            Diagnosis = record.Diagnosis,
                            MedicalRecordDate = record.Date.ToString("dd/MM/yyyy")
                        }).ToList()
                    }).ToList()
            };

            return Ok(recordVm);
        }
        [HttpGet("doctor-names")]
        public IActionResult GetDoctorNames()
        {
            var names = _medicalRecordService.QueryAll()
                .Where(r => r.User != null && !string.IsNullOrEmpty(r.User.FullName))
                .Select(r => r.User.FullName!)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            return Ok(names);
        }
        [HttpGet("patient-names")]
        public IActionResult GetPatientNames()
        {
            var names = _medicalRecordService.QueryAll()
                .Where(r => r.Patient != null && !string.IsNullOrEmpty(r.Patient.FullName))
                .Select(r => r.Patient.FullName!)
                .Distinct()
                .OrderBy(n => n)
                .ToList();

            return Ok(names);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var records = _medicalRecordService.QueryAll()
                .ToList() // ✅ Ép IQueryable về List
                .Select(r => new MedicalRecordVM
                {
                    RecordId = r.RecordId,
                    PatientId = r.PatientId,
                    PatientName = r.Patient != null ? r.Patient.FullName : null, // ✅ Không dùng ?.
                    UserId = r.UserId,
                    DoctorName = r.User != null ? r.User.FullName : null,        // ✅ Không dùng ?.
                    Date = r.Date,
                    Symptoms = r.Symptoms,
                    Diagnosis = r.Diagnosis,
                    Note = r.Note
                }).ToList();

            return Ok(records);
        }


    }
}
