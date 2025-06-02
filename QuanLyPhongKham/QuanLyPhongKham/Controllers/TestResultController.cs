using BusinessAccessLayer.IService;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly ITestResultService _testResultService;

        public TestResultController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        // GET: api/TestResult
        [HttpGet]
        public ActionResult<List<TestResultVM>> GetAllTestResults()
        {
            try
            {
                var testResults = _testResultService.GetAllTestResultVMs();
                return Ok(testResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving test results", error = ex.Message });
            }
        }

        // GET: api/TestResult/{id}
        [HttpGet("{id}")]
        public ActionResult<TestResult> GetTestResult(int id)
        {
            try
            {
                var testResult = _testResultService.GetTestResultById(id);
                if (testResult == null)
                {
                    return NotFound(new { message = "Test result not found" });
                }
                return Ok(testResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the test result", error = ex.Message });
            }
        }

        // GET: api/TestResult/record/{recordId}
        [HttpGet("record/{recordId}")]
        public ActionResult<List<TestResult>> GetTestResultsByRecord(int recordId)
        {
            try
            {
                var testResults = _testResultService.GetTestResultsByRecordId(recordId);
                return Ok(testResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving test results by record", error = ex.Message });
            }
        }

        // GET: api/TestResult/user/{userId}
        [HttpGet("user/{userId}")]
        public ActionResult<List<TestResult>> GetTestResultsByUser(int userId)
        {
            try
            {
                var testResults = _testResultService.GetTestResultsByUserId(userId);
                return Ok(testResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving test results by user", error = ex.Message });
            }
        }

        // GET: api/TestResult/edit/{id}
        [HttpGet("edit/{id}")]
        public ActionResult<TestResultVM> GetTestResultForEdit(int id)
        {
            try
            {
                var testResultVM = _testResultService.GetTestResultForEdit(id);
                if (testResultVM == null)
                {
                    return NotFound(new { message = "Test result not found" });
                }
                return Ok(testResultVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving test result for edit", error = ex.Message });
            }
        }

        // GET: api/TestResult/new
        [HttpGet("new")]
        public ActionResult<TestResultVM> GetNewTestResult()
        {
            try
            {
                var newTestResultVM = _testResultService.GetNewTestResultVM();
                return Ok(newTestResultVM);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating new test result template", error = ex.Message });
            }
        }

        // POST: api/TestResult
        [HttpPost]
        public ActionResult CreateTestResult([FromBody] TestResultVM testResultVM)
        {
            try
            {
                if (testResultVM == null)
                {
                    return BadRequest(new { message = "Test result data is required" });
                }

                if (!_testResultService.ValidateTestResult(testResultVM, out List<string> errors))
                {
                    return BadRequest(new { message = "Validation failed", errors = errors });
                }

                bool result = _testResultService.CreateTestResult(testResultVM);
                if (result)
                {
                    return Ok(new { message = "Test result created successfully" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to create test result" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating test result", error = ex.Message });
            }
        }

        // PUT: api/TestResult/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateTestResult(int id, [FromBody] TestResultVM testResultVM)
        {
            try
            {
                if (testResultVM == null)
                {
                    return BadRequest(new { message = "Test result data is required" });
                }

                if (!_testResultService.TestResultExists(id))
                {
                    return NotFound(new { message = "Test result not found" });
                }

                if (!_testResultService.ValidateTestResult(testResultVM, out List<string> errors))
                {
                    return BadRequest(new { message = "Validation failed", errors = errors });
                }

                bool result = _testResultService.UpdateTestResult(id, testResultVM);
                if (result)
                {
                    return Ok(new { message = "Test result updated successfully" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to update test result" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating test result", error = ex.Message });
            }
        }

        // DELETE: api/TestResult/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTestResult(int id)
        {
            try
            {
                if (!_testResultService.TestResultExists(id))
                {
                    return NotFound(new { message = "Test result not found" });
                }

                bool result = _testResultService.DeleteTestResult(id);
                if (result)
                {
                    return Ok(new { message = "Test result deleted successfully" });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to delete test result" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting test result", error = ex.Message });
            }
        }

        // GET: api/TestResult/exists/{id}
        [HttpGet("exists/{id}")]
        public ActionResult<bool> TestResultExists(int id)
        {
            try
            {
                bool exists = _testResultService.TestResultExists(id);
                return Ok(new { exists = exists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while checking test result existence", error = ex.Message });
            }
        }
    }
}