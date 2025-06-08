using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace QuanLyPhongKham.Pages.TestResultPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public IndexModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7086";
        }

        public List<TestResultVM> TestResults { get; set; } = new List<TestResultVM>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/TestResult");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    TestResults = JsonConvert.DeserializeObject<List<TestResultVM>>(content) ?? new List<TestResultVM>();
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to load test results. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading test results: {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                // First check if test result exists
                var existsResponse = await _httpClient.GetAsync($"{_apiBaseUrl}/api/TestResult/exists/{id}");
                if (!existsResponse.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Test result not found.";
                    return RedirectToPage();
                }

                // Delete the test result
                var deleteResponse = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/TestResult/{id}");

                if (deleteResponse.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Test result deleted successfully.";
                }
                else
                {
                    var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                    TempData["ErrorMessage"] = errorObj?.message?.ToString() ?? "Failed to delete test result.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting test result: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}