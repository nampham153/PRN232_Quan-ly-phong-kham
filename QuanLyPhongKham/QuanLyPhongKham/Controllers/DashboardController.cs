using DataAccessLayer.dbcontext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ClinicDbContext _context;

    public DashboardController(ClinicDbContext context)
    {
        _context = context;
    }

    // 1. Tổng số loại xét nghiệm
    [HttpGet("test-count")]
    public async Task<IActionResult> GetTestCount()
    {
        var count = await _context.Tests.CountAsync();
        return Ok(new { count });
    }

    // 2. Tổng số kết quả xét nghiệm
    [HttpGet("test-result-count")]
    public async Task<IActionResult> GetTestResultCount()
    {
        var count = await _context.TestResults.CountAsync();
        return Ok(new { count });
    }

    // 3. Kết quả xét nghiệm theo ngày
    [HttpGet("results-by-date")]
    public async Task<IActionResult> GetResultsByDate()
    {
        var results = await _context.TestResults
            .GroupBy(r => r.TestDate.Date)
            .Select(g => new
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.Date)
            .ToListAsync();

        // xử lý format ngày ở client (JS) hoặc ở đây dùng LINQ client-side
        var formattedResults = results
            .AsEnumerable() // chuyển sang client
            .Select(r => new
            {
                date = r.Date.ToString("yyyy-MM-dd"),
                count = r.Count
            });

        return Ok(formattedResults);
    }



    // 4. Phổ biến các loại xét nghiệm
    [HttpGet("test-type-distribution")]
    public async Task<IActionResult> GetTestTypeDistribution()
    {
        var data = await _context.TestResults
            .Include(tr => tr.Test)
            .GroupBy(tr => tr.Test.TestName)
            .Select(g => new
            {
                testName = g.Key,
                count = g.Count()
            })
            .OrderByDescending(x => x.count)
            .Take(10)
            .ToListAsync();

        return Ok(data);
    }

    // 5. Số lượt xét nghiệm theo người thực hiện (UserId)
    [HttpGet("results-by-user")]
    public async Task<IActionResult> GetResultsByUser()
    {
        var data = await _context.TestResults
            .Include(tr => tr.User)
            .GroupBy(tr => tr.User.FullName)
            .Select(g => new
            {
                user = g.Key,
                count = g.Count()
            })
            .OrderByDescending(x => x.count)
            .ToListAsync();

        return Ok(data);
    }

    // 6. Số lượt xét nghiệm theo bác sĩ (User.DoctorPath != null)
    [HttpGet("tests-by-doctor")]
    public IActionResult GetTestsByDoctor()
    {
        var data = _context.TestResults
            .Include(tr => tr.User)
            .GroupBy(tr => tr.User.FullName)
            .Select(g => new {
                doctor = g.Key,
                count = g.Count()
            }).ToList();

        return Ok(data);
    }


    // 7. Thống kê giới tính + độ tuổi bệnh nhân (qua User table)
    [HttpGet("results-by-age-gender")]
    public async Task<IActionResult> GetResultsByAgeGender()
    {
        var now = DateTime.Now;

        var testResults = await _context.TestResults
            .Include(tr => tr.User)
            .Where(tr => tr.User.DOB != null && tr.User.Gender != null)
            .ToListAsync();

        var grouped = testResults
            .GroupBy(tr =>
            {
                var age = (int)((now - tr.User.DOB.Value).TotalDays / 365.25);
                string ageGroup = age switch
                {
                    <= 17 => "0-17",
                    <= 30 => "18-30",
                    <= 45 => "31-45",
                    <= 60 => "46-60",
                    _ => "60+"
                };
                return $"{tr.User.Gender} - {ageGroup}";
            })
            .Select(g => new
            {
                label = g.Key,
                count = g.Count()
            }).ToList();

        return Ok(grouped);
    }

}
