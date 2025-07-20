using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyPhongKham.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestResultHistoryController : ControllerBase
    {
        private readonly ITestResultHistoryService _historyService;
        public TestResultHistoryController(ITestResultHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_historyService.GetAllHistoryVMs());
        }

        [HttpGet("by-user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            return Ok(_historyService.GetHistoriesByUserId(userId));
        }

        [HttpGet("by-testresult/{testResultId}")]
        public IActionResult GetByTestResult(int testResultId)
        {
            return Ok(_historyService.GetHistoriesByTestResultId(testResultId));
        }

        [HttpPost]
        public IActionResult Add([FromBody] TestResultHistoryVM vm)
        {
            if (_historyService.AddHistory(vm))
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_historyService.DeleteHistory(id))
                return Ok();
            return NotFound();
        }
    }
}