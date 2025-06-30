using BusinessAccessLayer.IService;
using BusinessAccessLayer.Service;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TestResultController : ControllerBase
    {
        private readonly ITestResultService _testResultService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IDoctorService _doctorService;
        private readonly ITestResultPdfService _pdfService;
        private readonly HttpClient _httpClient;

        public TestResultController(
            ITestResultService testResultService,
            IMedicalRecordService medicalRecordService,
            IDoctorService doctorService,
            ITestResultPdfService pdfService,
            HttpClient httpClient)
        {
            _testResultService = testResultService;
            _medicalRecordService = medicalRecordService;
            _doctorService = doctorService;
            _pdfService = pdfService;
            _httpClient = httpClient;
        }

        [HttpGet("odata")]
        [EnableQuery]
        public IQueryable<TestResult> GetTestResultsOData()
        {
            try
            {
                var testResults = _testResultService.GetAllTestResults().AsQueryable();
                Console.WriteLine($"GetTestResultsOData: Returned {testResults.Count()} records");
                return testResults;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTestResultsOData: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return new List<TestResult>().AsQueryable();
            }
        }

        [HttpGet]
        public IActionResult GetAllTestResults(
            [FromQuery] string? testName = null,
            [FromQuery] string? userName = null,
            [FromQuery] string? patientName = null,
            [FromQuery] int? recordId = null,
            [FromQuery] int? userId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string sortBy = "ResultId", // Changed default to ResultId
            [FromQuery] bool sortDescending = false, // Changed default to false for ascending order
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                Console.WriteLine($"GetAllTestResults called with: testName={testName}, userName={userName}, patientName={patientName}, recordId={recordId}, userId={userId}, fromDate={fromDate}, toDate={toDate}, sortBy={sortBy}, sortDescending={sortDescending}, page={page}, pageSize={pageSize}");

                var allTestResults = _testResultService.GetAllTestResults();
                Console.WriteLine($"TestResultService returned {allTestResults?.Count() ?? 0} records");

                if (allTestResults == null || !allTestResults.Any())
                {
                    Console.WriteLine("No test results found in service");
                    return Ok(new
                    {
                        Data = new List<TestResultVM>(),
                        TotalRecords = 0,
                        Page = page,
                        PageSize = pageSize,
                        Message = "No test results found"
                    });
                }

                var testResultsList = allTestResults.ToList();
                var filteredResults = testResultsList.AsQueryable();

                if (!string.IsNullOrEmpty(testName))
                {
                    filteredResults = filteredResults.Where(x => x.Test != null &&
                        x.Test.TestName.Contains(testName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    filteredResults = filteredResults.Where(x => x.User != null &&
                        x.User.FullName.Contains(userName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(patientName))
                {
                    filteredResults = filteredResults.Where(x => x.MedicalRecord != null &&
                        x.MedicalRecord.Patient != null &&
                        x.MedicalRecord.Patient.FullName.Contains(patientName, StringComparison.OrdinalIgnoreCase));
                }

                if (recordId.HasValue)
                {
                    filteredResults = filteredResults.Where(x => x.RecordId == recordId.Value);
                }

                if (userId.HasValue)
                {
                    filteredResults = filteredResults.Where(x => x.UserId == userId.Value);
                }

                if (fromDate.HasValue)
                {
                    filteredResults = filteredResults.Where(x => x.TestDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    filteredResults = filteredResults.Where(x => x.TestDate <= toDate.Value);
                }

                switch (sortBy.ToLower())
                {
                    case "resultid":
                        filteredResults = sortDescending
                            ? filteredResults.OrderByDescending(x => x.ResultId)
                            : filteredResults.OrderBy(x => x.ResultId);
                        break;
                    case "testdate":
                        filteredResults = sortDescending
                            ? filteredResults.OrderByDescending(x => x.TestDate)
                            : filteredResults.OrderBy(x => x.TestDate);
                        break;
                    case "testname":
                        filteredResults = sortDescending
                            ? filteredResults.OrderByDescending(x => x.Test != null ? x.Test.TestName : "")
                            : filteredResults.OrderBy(x => x.Test != null ? x.Test.TestName : "");
                        break;
                    case "username":
                        filteredResults = sortDescending
                            ? filteredResults.OrderByDescending(x => x.User != null ? x.User.FullName : "")
                            : filteredResults.OrderBy(x => x.User != null ? x.User.FullName : "");
                        break;
                    case "patientname":
                        filteredResults = sortDescending
                            ? filteredResults.OrderByDescending(x => x.MedicalRecord != null && x.MedicalRecord.Patient != null ? x.MedicalRecord.Patient.FullName : "")
                            : filteredResults.OrderBy(x => x.MedicalRecord != null && x.MedicalRecord.Patient != null ? x.MedicalRecord.Patient.FullName : "");
                        break;
                    default:
                        filteredResults = sortDescending
                            ? filteredResults.OrderByDescending(x => x.ResultId)
                            : filteredResults.OrderBy(x => x.ResultId);
                        break;
                }

                var totalRecords = filteredResults.Count();
                var pagedResults = filteredResults
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var data = pagedResults.Select(x => new TestResultVM
                {
                    RecordId = x.RecordId,
                    TestId = x.TestId,
                    UserId = x.UserId,
                    ResultDetail = x.ResultDetail ?? "",
                    TestDate = x.TestDate,
                    TestName = x.Test?.TestName ?? "N/A",
                    TestDescription = x.Test?.Description ?? "",
                    UserName = x.User?.FullName ?? "N/A",
                    PatientName = x.MedicalRecord?.Patient?.FullName ?? "N/A",
                    Diagnosis = x.MedicalRecord?.Diagnosis ?? "",
                    MedicalRecordDate = x.MedicalRecord?.Date.ToString("dd/MM/yyyy") ?? ""
                }).ToList();

                var response = new
                {
                    Data = data,
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize
                };

                Console.WriteLine($"Returning {data.Count} test results, TotalRecords: {totalRecords}, Page: {page}, PageSize: {pageSize}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTestResults: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    message = "Error retrieving test results",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("with-resultid")]
        public IActionResult GetTestResultsWithResultId()
        {
            try
            {
                var testResults = _testResultService.GetAllTestResults();
                Console.WriteLine($"GetTestResultsWithResultId: Service returned {testResults?.Count() ?? 0} records");
                if (testResults == null || !testResults.Any())
                {
                    return Ok(new List<TestResult>());
                }

                var results = testResults
                    .Select(x => new TestResult
                    {
                        ResultId = x.ResultId,
                        RecordId = x.RecordId,
                        TestId = x.TestId,
                        UserId = x.UserId,
                        ResultDetail = x.ResultDetail,
                        TestDate = x.TestDate
                    })
                    .ToList();

                Console.WriteLine($"Returning {results.Count} test results with ResultId");
                return Ok(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTestResultsWithResultId: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving test results with ResultId", error = ex.Message });
            }
        }

        [HttpGet("test")]
        public IActionResult TestConnection()
        {
            try
            {
                var testResults = _testResultService.GetAllTestResults();
                var count = testResults?.Count() ?? 0;
                Console.WriteLine($"TestConnection: Service returned {count} records");
                return Ok(new
                {
                    message = "Service connection test successful",
                    count = count,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TestConnection: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new
                {
                    message = "Service connection test failed",
                    error = ex.Message
                });
            }
        }

        [HttpGet("test-names")]
        public IActionResult GetDistinctTestNames()
        {
            try
            {
                var testNames = _testResultService.GetAllTestResults()
                    .Where(x => x.Test != null && !string.IsNullOrEmpty(x.Test.TestName))
                    .Select(x => x.Test.TestName)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                Console.WriteLine($"GetDistinctTestNames: Returning {testNames.Count} test names");
                return Ok(testNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDistinctTestNames: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving test names", error = ex.Message });
            }
        }

        [HttpGet("doctor-names")]
        public IActionResult GetDistinctDoctorNames()
        {
            try
            {
                var doctorNames = _testResultService.GetAllTestResults()
                    .Where(x => x.User != null && !string.IsNullOrEmpty(x.User.FullName))
                    .Select(x => x.User.FullName)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                Console.WriteLine($"GetDistinctDoctorNames: Returning {doctorNames.Count} doctor names");
                return Ok(doctorNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDistinctDoctorNames: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving doctor names", error = ex.Message });
            }
        }

        [HttpGet("patient-names")]
        public IActionResult GetDistinctPatientNames()
        {
            try
            {
                var patientNames = _testResultService.GetAllTestResults()
                    .Where(x => x.MedicalRecord != null && x.MedicalRecord.Patient != null && !string.IsNullOrEmpty(x.MedicalRecord.Patient.FullName))
                    .Select(x => x.MedicalRecord.Patient.FullName)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                Console.WriteLine($"GetDistinctPatientNames: Returning {patientNames.Count} patient names");
                return Ok(patientNames);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDistinctPatientNames: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving patient names", error = ex.Message });
            }
        }

        [HttpGet("medical-records")]
        public IActionResult GetAvailableMedicalRecords()
        {
            try
            {
                var records = _medicalRecordService.QueryAll()
                    .Select(r => new
                    {
                        RecordId = r.RecordId,
                        PatientName = r.Patient != null ? r.Patient.FullName : "",
                        DoctorName = r.User != null ? r.User.FullName : "",
                        Date = r.Date.ToString("dd/MM/yyyy"),
                        Diagnosis = r.Diagnosis,
                        DisplayText = $"{(r.Patient != null ? r.Patient.FullName : "")} - {r.Date:dd/MM/yyyy} - {r.Diagnosis}"
                    })
                    .OrderByDescending(r => r.RecordId)
                    .ToList();
                Console.WriteLine($"GetAvailableMedicalRecords: Returning {records.Count} medical records");
                return Ok(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableMedicalRecords: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving medical records", error = ex.Message });
            }
        }

        [HttpGet("doctors")]
        public IActionResult GetAvailableDoctors()
        {
            try
            {
                var doctors = _doctorService.GetAllDoctors()
                    .Select(d => new
                    {
                        UserId = d.UserId,
                        FullName = d.FullName,
                        Email = d.Email,
                        Phone = d.Phone,
                        DoctorPath = d.DoctorPath
                    })
                    .OrderBy(d => d.FullName)
                    .ToList();
                Console.WriteLine($"GetAvailableDoctors: Returning {doctors.Count} doctors");
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableDoctors: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving doctors", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTestResult(int id)
        {
            try
            {
                var testResult = _testResultService.GetTestResultById(id);
                if (testResult == null)
                {
                    Console.WriteLine($"GetTestResult: Test result with ID {id} not found");
                    return NotFound(new { message = "Test result not found" });
                }

                var vm = new TestResultVM
                {
                    RecordId = testResult.RecordId,
                    TestId = testResult.TestId,
                    UserId = testResult.UserId,
                    ResultDetail = testResult.ResultDetail ?? "",
                    TestDate = testResult.TestDate,
                    TestName = testResult.Test?.TestName ?? "",
                    TestDescription = testResult.Test?.Description ?? "",
                    UserName = testResult.User?.FullName ?? "",
                    PatientName = testResult.MedicalRecord?.Patient?.FullName ?? "",
                    Diagnosis = testResult.MedicalRecord?.Diagnosis ?? "",
                    MedicalRecordDate = testResult.MedicalRecord?.Date.ToString("dd/MM/yyyy") ?? ""
                };
                Console.WriteLine($"GetTestResult: Returning test result with ID {id}");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTestResult: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving test result", error = ex.Message });
            }
        }

        [HttpGet("record/{recordId}")]
        public IActionResult GetByRecord(int recordId)
        {
            try
            {
                var testResults = _testResultService.GetTestResultsByRecordId(recordId);
                Console.WriteLine($"GetByRecord: Returned {testResults?.Count() ?? 0} test results for RecordId {recordId}");
                var vm = testResults.Select(tr => new TestResultVM
                {
                    RecordId = tr.RecordId,
                    TestId = tr.TestId,
                    UserId = tr.UserId,
                    ResultDetail = tr.ResultDetail ?? "",
                    TestDate = tr.TestDate,
                    TestName = tr.Test?.TestName ?? "",
                    TestDescription = tr.Test?.Description ?? "",
                    UserName = tr.User?.FullName ?? "",
                    PatientName = tr.MedicalRecord?.Patient?.FullName ?? "",
                    Diagnosis = tr.MedicalRecord?.Diagnosis ?? "",
                    MedicalRecordDate = tr.MedicalRecord?.Date.ToString("dd/MM/yyyy") ?? ""
                }).ToList();
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByRecord: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving by record", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            try
            {
                var testResults = _testResultService.GetTestResultsByUserId(userId);
                Console.WriteLine($"GetByUser: Returned {testResults?.Count() ?? 0} test results for UserId {userId}");
                var vm = testResults.Select(tr => new TestResultVM
                {
                    RecordId = tr.RecordId,
                    TestId = tr.TestId,
                    UserId = tr.UserId,
                    ResultDetail = tr.ResultDetail ?? "",
                    TestDate = tr.TestDate,
                    TestName = tr.Test?.TestName ?? "",
                    TestDescription = tr.Test?.Description ?? "",
                    UserName = tr.User?.FullName ?? "",
                    PatientName = tr.MedicalRecord?.Patient?.FullName ?? "",
                    Diagnosis = tr.MedicalRecord?.Diagnosis ?? "",
                    MedicalRecordDate = tr.MedicalRecord?.Date.ToString("dd/MM/yyyy") ?? ""
                }).ToList();
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByUser: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving by user", error = ex.Message });
            }
        }

        [HttpGet("edit/{id}")]
        public IActionResult GetForEdit(int id)
        {
            try
            {
                var vm = _testResultService.GetTestResultForEdit(id);
                if (vm == null)
                {
                    Console.WriteLine($"GetForEdit: Test result with ID {id} not found");
                    return NotFound(new { message = "Test result not found" });
                }
                Console.WriteLine($"GetForEdit: Returning test result for edit with ID {id}");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetForEdit: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error retrieving for edit", error = ex.Message });
            }
        }

        [HttpGet("new")]
        public IActionResult GetNew()
        {
            try
            {
                var vm = _testResultService.GetNewTestResultVM();
                Console.WriteLine("GetNew: Returning new TestResultVM template");
                return Ok(vm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNew: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error creating template", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] TestResultVM vm)
        {
            try
            {
                if (vm == null)
                {
                    Console.WriteLine("Create: Received null TestResultVM");
                    return BadRequest(new { message = "Data cannot be empty" });
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine($"Create: Invalid ModelState: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }

                if (!_testResultService.ValidateTestResult(vm, out var errors))
                {
                    Console.WriteLine($"Create: Validation failed: {string.Join(", ", errors)}");
                    return BadRequest(new { message = "Validation failed", errors });
                }

                if (_testResultService.CreateTestResult(vm))
                {
                    Console.WriteLine("Create: Test result created successfully");
                    return Ok(new { message = "Created successfully" });
                }

                Console.WriteLine("Create: Test result creation failed");
                return StatusCode(500, new { message = "Creation failed" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error creating test result", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TestResultVM vm)
        {
            try
            {
                if (vm == null)
                {
                    Console.WriteLine($"Update: Received null TestResultVM for ID {id}");
                    return BadRequest(new { message = "Data cannot be empty" });
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine($"Update: Invalid ModelState for ID {id}: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }

                if (!_testResultService.TestResultExists(id))
                {
                    Console.WriteLine($"Update: Test result with ID {id} not found");
                    return NotFound(new { message = "Test result not found" });
                }

                if (!_testResultService.ValidateTestResult(vm, out var errors))
                {
                    Console.WriteLine($"Update: Validation failed for ID {id}: {string.Join(", ", errors)}");
                    return BadRequest(new { message = "Validation failed", errors });
                }

                if (_testResultService.UpdateTestResult(id, vm))
                {
                    Console.WriteLine($"Update: Test result with ID {id} updated successfully");
                    return Ok(new { message = "Updated successfully" });
                }

                Console.WriteLine($"Update: Test result update failed for ID {id}");
                return StatusCode(500, new { message = "Update failed" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error updating test result", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_testResultService.TestResultExists(id))
                {
                    Console.WriteLine($"Delete: Test result with ID {id} not found");
                    return NotFound(new { message = "Test result not found" });
                }

                if (_testResultService.DeleteTestResult(id))
                {
                    Console.WriteLine($"Delete: Test result with ID {id} deleted successfully");
                    return Ok(new { message = "Deleted successfully" });
                }

                Console.WriteLine($"Delete: Test result deletion failed for ID {id}");
                return StatusCode(500, new { message = "Deletion failed" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error deleting test result", error = ex.Message });
            }
        }

        [HttpGet("exists/{id}")]
        public IActionResult Exists(int id)
        {
            try
            {
                var exists = _testResultService.TestResultExists(id);
                Console.WriteLine($"Exists: Test result with ID {id} exists: {exists}");
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Exists: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error checking existence", error = ex.Message });
            }
        }

        
        [HttpGet("pdf/{id}")]
        public IActionResult GeneratePdf(int id)
        {
            try
            {
                var testResult = _testResultService.GetTestResultById(id);
                if (testResult == null)
                {
                    Console.WriteLine($"GeneratePdf: Test result with ID {id} not found");
                    return NotFound(new { message = "Test result not found" });
                }

                var vm = new TestResultVM
                {
                    RecordId = testResult.RecordId,
                    TestId = testResult.TestId,
                    UserId = testResult.UserId,
                    ResultDetail = testResult.ResultDetail ?? "",
                    TestDate = testResult.TestDate,
                    TestName = testResult.Test?.TestName ?? "",
                    TestDescription = testResult.Test?.Description ?? "",
                    UserName = testResult.User?.FullName ?? "",
                    PatientName = testResult.MedicalRecord?.Patient?.FullName ?? "",
                    Diagnosis = testResult.MedicalRecord?.Diagnosis ?? "",
                    MedicalRecordDate = testResult.MedicalRecord?.Date.ToString("dd/MM/yyyy") ?? ""
                };

                var pdfBytes = _pdfService.GenerateTestResultPdf(vm);
                var fileName = $"KetQuaXetNghiem_{vm.PatientName?.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                Console.WriteLine($"GeneratePdf: Generated PDF for test result ID {id}");
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GeneratePdf: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error generating PDF", error = ex.Message });
            }
        }

        
        [HttpGet("pdf/preview/{id}")]
        public IActionResult PreviewPdf(int id)
        {
            try
            {
                var testResult = _testResultService.GetTestResultById(id);
                if (testResult == null)
                {
                    Console.WriteLine($"PreviewPdf: Test result with ID {id} not found");
                    return NotFound(new { message = "Test result not found" });
                }

                var vm = new TestResultVM
                {
                    RecordId = testResult.RecordId,
                    TestId = testResult.TestId,
                    UserId = testResult.UserId,
                    ResultDetail = testResult.ResultDetail ?? "",
                    TestDate = testResult.TestDate,
                    TestName = testResult.Test?.TestName ?? "",
                    TestDescription = testResult.Test?.Description ?? "",
                    UserName = testResult.User?.FullName ?? "",
                    PatientName = testResult.MedicalRecord?.Patient?.FullName ?? "",
                    Diagnosis = testResult.MedicalRecord?.Diagnosis ?? "",
                    MedicalRecordDate = testResult.MedicalRecord?.Date.ToString("dd/MM/yyyy") ?? ""
                };

                var pdfBytes = _pdfService.GenerateTestResultPdf(vm);

                Console.WriteLine($"PreviewPdf: Generated PDF preview for test result ID {id}");
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PreviewPdf: {ex.Message}\nStack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error generating PDF preview", error = ex.Message });
            }
        }
    }
}