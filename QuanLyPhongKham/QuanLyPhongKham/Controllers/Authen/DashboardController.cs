using DataAccessLayer.dbcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace QuanLyPhongKham.Controllers.Authen
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ClinicDbContext _context;

        public DashboardController(ClinicDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Admin 
        /// </summary>
        /// <returns></returns>
        [HttpGet("account-by-status")]
        public async Task<IActionResult> GetAccountByStatus()
        {
            var result = await _context.Accounts
                .GroupBy(a => a.Status)
                .Select(g => new {
                    Status = g.Key ? "Active" : "Inactive",
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("account-by-role")]
        public async Task<IActionResult> GetAccountCountsByRole()
        {
            var data = await _context.Accounts
                .Include(a => a.Role)
                .GroupBy(a => a.Role.RoleName)
                .Select(g => new
                {
                    Role = g.Key,
                    Count = g.Count()
                }).ToListAsync();

            return Ok(data);
        }


        [HttpGet("account-by-day")]
        public async Task<IActionResult> GetAccountCreatedByDay([FromQuery] int days = 30)
        {
            var fromDate = DateTime.Today.AddDays(-days);

            var result = await _context.Accounts
                .Where(a => a.CreatedAt.Date >= fromDate)
                .GroupBy(a => a.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var formatted = result
                .AsEnumerable()
                .Select(x => new
                {
                    Day = x.Date.ToString("dd/MM/yyyy"),
                    x.Count
                });

            return Ok(formatted);
        }


        [HttpGet("account-by-month")]
        public async Task<IActionResult> GetAccountCreatedByMonth([FromQuery] int months = 12)
        {
            var fromDate = DateTime.Today.AddMonths(-months);

            var result = await _context.Accounts
                .Where(a => a.CreatedAt >= fromDate)
                .GroupBy(a => new { a.CreatedAt.Year, a.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            var formatted = result
                .AsEnumerable()
                .Select(x => new
                {
                    Month = $"{x.Month:D2}/{x.Year}",
                    x.Count
                });

            return Ok(formatted);
        }


        ////Patient

        //  Tổng số bệnh nhân
        [HttpGet("count")]
        public ActionResult<int> GetPatientCount()
        {
            var count = _context.Patients.Count();
            return Ok(count);
        }

        // Thống kê giới tính
        [HttpGet("gender-count")]
        public ActionResult GetGenderCount()
        {
            var data = _context.Patients
                .GroupBy(p => p.Gender)
                .Select(g => new { Gender = g.Key, Count = g.Count() })
                .ToList();

            return Ok(data);
        }

        //  Thống kê theo nhóm tuổi
        [HttpGet("age-groups")]
        public ActionResult GetAgeGroups()
        {
            var today = DateTime.Today;

            var data = _context.Patients
                .Where(p => p.DOB != null)
                .Select(p => new
                {
                    Age = today.Year - p.DOB.Value.Year -
                          (today < p.DOB.Value.AddYears(today.Year - p.DOB.Value.Year) ? 1 : 0)
                })
                .AsEnumerable()
                .GroupBy(a => a.Age switch
                {
                    <= 10 => "1-10",
                    <= 22 => "11-22",
                    <= 40 => "23-40",
                    <= 55 => "41-55",
                    <= 75 => "56-75",
                    _ => "76-100"
                })
                .Select(g => new { Group = g.Key, Count = g.Count() })
                .ToList();

            return Ok(data);
        }


        //Medical
        [HttpGet("most-prescribed-medicines")]
        public IActionResult GetMostPrescribedMedicines(DateTime? from = null, DateTime? to = null)
        {
            var prescriptions = _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                prescriptions = prescriptions.Where(p =>
                    p.MedicalRecord.Date >= from.Value && p.MedicalRecord.Date <= to.Value);
            }

            var result = prescriptions
                .GroupBy(p => p.Medicine.MedicineName)
                .Select(g => new
                {
                    Medicine = g.Key,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .ToList();

            return Ok(result);
        }


        [HttpGet("top-doctors-prescribing")]
        public IActionResult GetTopDoctors(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.MedicalRecord)
                    .ThenInclude(mr => mr.User)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p =>
                    p.MedicalRecord.Date >= from.Value &&
                    p.MedicalRecord.Date <= to.Value);
            }

            var result = query
                .GroupBy(p => p.MedicalRecord.User.FullName)
                .Select(g => new
                {
                    Doctor = g.Key,
                    TotalMedicinesPrescribed = g.Count()
                })
                .OrderByDescending(x => x.TotalMedicinesPrescribed)
                .ToList();

            return Ok(result);
        }

        [HttpGet("monthly-prescription-totals")]
        public IActionResult GetMonthlyTotals(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p => p.MedicalRecord.Date >= from.Value && p.MedicalRecord.Date <= to.Value);
            }

            var result = query
                .GroupBy(p => p.MedicalRecord.Date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalQuantity = g.Sum(p => p.Quantity)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(result);
        }


        [HttpGet("medicine-usage-monthly")]
        public IActionResult GetMedicineFrequency(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p => p.MedicalRecord.Date >= from.Value && p.MedicalRecord.Date <= to.Value);
            }

            var result = query
                .GroupBy(p => new { p.Medicine.MedicineName, Month = p.MedicalRecord.Date.Month })
                .Select(g => new
                {
                    Medicine = g.Key.MedicineName,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(g => g.Month)
                .ThenBy(g => g.Medicine)
                .ToList();

            return Ok(result);
        }


        [HttpGet("popular-units")]
        public IActionResult GetPopularUnits(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p =>
                    p.MedicalRecord.Date >= from.Value &&
                    p.MedicalRecord.Date <= to.Value);
            }

            var result = query
                .GroupBy(p => p.Medicine.Unit)
                .Select(g => new
                {
                    Unit = g.Key,
                    UsageCount = g.Count()
                })
                .OrderByDescending(g => g.UsageCount)
                .ToList();

            return Ok(result);
        }

        [HttpGet("daily-prescription-totals")]
        public IActionResult GetDailyPrescriptionTotals(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p => p.MedicalRecord.Date.Date >= from.Value.Date &&
                                         p.MedicalRecord.Date.Date <= to.Value.Date);
            }

            var result = query
                .GroupBy(p => p.MedicalRecord.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalQuantity = g.Sum(p => p.Quantity)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return Ok(result);
        }

        [HttpGet("daily-medicine-usage")]
        public IActionResult GetDailyMedicineUsage(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p => p.MedicalRecord.Date.Date >= from.Value.Date &&
                                         p.MedicalRecord.Date.Date <= to.Value.Date);
            }

            var result = query
                .GroupBy(p => new { Date = p.MedicalRecord.Date.Date, Medicine = p.Medicine.MedicineName })
                .Select(g => new
                {
                    g.Key.Date,
                    g.Key.Medicine,
                    TotalQuantity = g.Sum(p => p.Quantity)
                })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.Medicine)
                .ToList();

            return Ok(result);
        }


        [HttpGet("all-medicines-total")]
        public IActionResult GetAllMedicinesTotal(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.MedicalRecord)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(p => p.MedicalRecord.Date >= from.Value && p.MedicalRecord.Date <= to.Value);
            }

            var result = query
                .GroupBy(p => p.Medicine.MedicineName)
                .Select(g => new
                {
                    Medicine = g.Key,
                    TotalSold = g.Sum(p => p.Quantity)
                })
                .OrderByDescending(g => g.TotalSold)
                .ToList();

            return Ok(result);
        }



        //Medical Record
        // Tổng số lượt khám
        [HttpGet("total-medical-records")]
        public IActionResult GetTotalMedicalRecords()
        {
            var total = _context.MedicalRecords.Count();
            return Ok(new { TotalRecords = total });
        }



        // Lượt khám theo ngày
        [HttpGet("records-daily")]
        public IActionResult GetRecordsByDay(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.MedicalRecords.AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(r => r.Date.Date >= from.Value.Date && r.Date.Date <= to.Value.Date);
            }

            var result = query
                .GroupBy(r => r.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,     // lấy kiểu DateTime thô
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .AsEnumerable()       // chuyển về xử lý client
                .Select(g => new
                {
                    Date = g.Date.ToString("yyyy-MM-dd"),  // format ở đây an toàn
                    Count = g.Count
                })
                .ToList();

            return Ok(result);
        }



        // Lượt khám theo bác sĩ
        [HttpGet("records-by-doctor")]
        public IActionResult GetRecordsByDoctor(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.MedicalRecords
                .Include(r => r.User)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(r => r.Date.Date >= from.Value.Date && r.Date.Date <= to.Value.Date);
            }

            var result = query
                .GroupBy(r => r.User.FullName)
                .Select(g => new
                {
                    Doctor = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return Ok(result);
        }


        // Lượt khám theo bệnh nhân
        [HttpGet("records-by-patient")]
        public IActionResult GetRecordsByPatient(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.MedicalRecords
                .Include(r => r.Patient)
                .AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(r => r.Date.Date >= from.Value.Date && r.Date.Date <= to.Value.Date);
            }

            var result = query
                .GroupBy(r => r.Patient.FullName)
                .Select(g => new
                {
                    Patient = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return Ok(result);
        }


        // Chẩn đoán phổ biến
        [HttpGet("popular-diagnoses")]
        public IActionResult GetPopularDiagnoses(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.MedicalRecords.AsQueryable();

            if (from.HasValue && to.HasValue)
            {
                query = query.Where(r => r.Date.Date >= from.Value.Date && r.Date.Date <= to.Value.Date);
            }

            var result = query
                .Where(x => !string.IsNullOrEmpty(x.Diagnosis))
                .GroupBy(r => r.Diagnosis)
                .Select(g => new
                {
                    Diagnosis = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return Ok(result);
        }




    }


}



