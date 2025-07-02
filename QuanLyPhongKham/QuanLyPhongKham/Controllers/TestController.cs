using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using DataAccessLayer.ViewModels.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]

    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly ITestResultService _testResultService;

        public TestController(ITestService testService, ITestResultService testResultService)
        {
            _testService = testService;
            _testResultService = testResultService;
        }

        // GET: api/Test
        [HttpGet]
        public IActionResult GetAllTests()
        {
            try
            {
                var tests = _testService.GetAllTests();
                return Ok(tests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/Test/search?searchTerm=keyword
        [HttpGet("search")]
        public IActionResult SearchTests([FromQuery] string searchTerm)
        {
            try
            {
                var tests = _testService.SearchTests(searchTerm);
                return Ok(new
                {
                    searchTerm = searchTerm,
                    count = tests.Count,
                    data = tests
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/Test/filter?searchTerm=keyword&sortBy=TestName&sortDescending=false&pageNumber=1&pageSize=10
        [HttpGet("filter")]
        public IActionResult GetTestsWithFilter([FromQuery] SearchFilterVM filter)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = _testService.GetTestsWithFilter(filter ?? new SearchFilterVM());
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/Test/advanced-search - cho complex search
        [HttpPost("advanced-search")]
        public IActionResult AdvancedSearch([FromBody] SearchFilterVM searchFilter)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = _testService.GetTestsWithFilter(searchFilter);
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/Test/5
        [HttpGet("{id}")]
        public IActionResult GetTest(int id)
        {
            try
            {
                var test = _testService.GetTestById(id);
                return Ok(test);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // POST: api/Test
        [HttpPost]
        public IActionResult CreateTest([FromBody] TestVM testVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _testService.CreateTest(testVM);
                return Ok(new { message = "Test created successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public IActionResult UpdateTest(int id, [FromBody] TestVM testVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _testService.UpdateTest(id, testVM);
                return Ok(new { message = "Test updated successfully" });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // DELETE: api/Test/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTest(int id)
        {
            try
            {
                // Kiểm tra xem Test có tồn tại không
                var test = _testService.GetTestById(id);
                if (test == null)
                {
                    return NotFound(new { message = "Test not found" });
                }

                // Kiểm tra xem Test có đang được sử dụng trong TestResult không
                var testResults = _testResultService.GetAllTestResults();
                var isTestInUse = testResults.Any(tr => tr.TestId == id);

                if (isTestInUse)
                {
                    // Đếm số lượng TestResult đang sử dụng Test này
                    var usageCount = testResults.Count(tr => tr.TestId == id);

                    return BadRequest(new
                    {
                        message = "Cannot delete test because it is being used in test results",
                        details = $"This test is currently used in {usageCount} test result(s). Please remove or update those test results first before deleting this test.",
                        usageCount = usageCount
                    });
                }

                // Nếu Test không được sử dụng, cho phép xóa
                _testService.DeleteTest(id);
                return Ok(new { message = "Test deleted successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // GET: api/Test/check-usage/5 - Endpoint bổ sung để kiểm tra việc sử dụng
        [HttpGet("check-usage/{id}")]
        public IActionResult CheckTestUsage(int id)
        {
            try
            {
                var test = _testService.GetTestById(id);
                if (test == null)
                {
                    return NotFound(new { message = "Test not found" });
                }

                var testResults = _testResultService.GetAllTestResults();
                var usageCount = testResults.Count(tr => tr.TestId == id);
                var isInUse = usageCount > 0;

                var usageDetails = testResults
                    .Where(tr => tr.TestId == id)
                    .Select(tr => new
                    {
                        ResultId = tr.ResultId,
                        TestDate = tr.TestDate,
                        PatientName = tr.MedicalRecord?.Patient?.FullName ?? "Unknown",
                        DoctorName = tr.User?.FullName ?? "Unknown"
                    })
                    .ToList();

                return Ok(new
                {
                    testId = id,
                    testName = test.TestName,
                    isInUse = isInUse,
                    usageCount = usageCount,
                    canDelete = !isInUse,
                    usageDetails = usageDetails
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}