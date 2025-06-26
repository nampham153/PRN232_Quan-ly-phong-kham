using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace QuanLyPhongKham.Pages.MedicalRecords
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7086";

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public MedicalRecordVM Record { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // L?y thông tin MedicalRecord
            var recordResponse = await client.GetAsync($"{_apiBaseUrl}/api/MedicalRecord/{id}");
            if (!recordResponse.IsSuccessStatusCode)
                return NotFound();

            var record = await recordResponse.Content.ReadFromJsonAsync<MedicalRecordVM>();
            if (record == null)
                return NotFound();

            Record = record;

            // L?y t?t c? TestResultVM t? API và l?c theo RecordId
            var testResultResponse = await client.GetAsync($"{_apiBaseUrl}/api/TestResult");
            if (testResultResponse.IsSuccessStatusCode)
            {
                var json = await testResultResponse.Content.ReadAsStringAsync();
                var allResults = JsonSerializer.Deserialize<List<TestResultVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<TestResultVM>();

                var recordResults = allResults
                    .Where(tr => tr.RecordId == Record.RecordId)
                    .ToList();

                // Group l?i theo ngày xét nghi?m
                Record.TestSummaries = recordResults
                    .GroupBy(tr => tr.TestDate.Date)
                    .OrderByDescending(g => g.Key)
                    .Select(g => new TestSummaryVM
                    {
                        TestDate = g.Key,
                        Results = g.ToList()
                    })
                    .ToList();
            }

            return Page();
        }
    }
}
