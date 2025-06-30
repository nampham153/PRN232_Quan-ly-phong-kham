using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace QuanLyPhongKhamFE.Pages.TestResultPage
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public EditModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7086";
        }

        [BindProperty]
        public TestResultVM TestResult { get; set; } = new TestResultVM();

        public List<SelectListItem> TestOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> UserOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MedicalRecordOptions { get; set; } = new List<SelectListItem>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                // Load the test result for editing
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/TestResult/edit/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Test result not found.";
                    return RedirectToPage("./Index");
                }

                var content = await response.Content.ReadAsStringAsync();
                TestResult = JsonConvert.DeserializeObject<TestResultVM>(content) ?? new TestResultVM();

                // Load dropdown options
                await LoadTestOptions();
                await LoadUserOptions();
                await LoadMedicalRecordOptions();

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading test result: {ex.Message}";
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                // Reload dropdown options in case of validation errors
                await LoadTestOptions();
                await LoadUserOptions();
                await LoadMedicalRecordOptions();

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Send update request
                var json = JsonConvert.SerializeObject(TestResult);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}/api/TestResult/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Test result updated successfully.";
                    return RedirectToPage("./Details", new { id = id });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var errorObj = JObject.Parse(errorContent);
                        if (errorObj["errors"] != null)
                        {
                            var errors = errorObj["errors"];
                            foreach (var error in errors)
                            {
                                ModelState.AddModelError("", error.ToString());
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", errorObj["message"]?.ToString() ?? "Update failed.");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Failed to update test result.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating test result: {ex.Message}");
            }

            return Page();
        }

        private async Task LoadTestOptions()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Test");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tests = JsonConvert.DeserializeObject<List<dynamic>>(content) ?? new List<dynamic>();

                    TestOptions = tests.Select(t => new SelectListItem
                    {
                        Value = t.testId.ToString(),
                        Text = t.testName?.ToString() ?? "Unknown Test"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                TestOptions = new List<SelectListItem>();
            }
        }

        private async Task LoadUserOptions()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/User");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<dynamic>>(content) ?? new List<dynamic>();

                    UserOptions = users.Select(u => new SelectListItem
                    {
                        Value = u.userId.ToString(),
                        Text = u.userName?.ToString() ?? "Unknown User"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                UserOptions = new List<SelectListItem>();
            }
        }

        private async Task LoadMedicalRecordOptions()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/MedicalRecord");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var records = JsonConvert.DeserializeObject<List<dynamic>>(content) ?? new List<dynamic>();

                    MedicalRecordOptions = records.Select(r => new SelectListItem
                    {
                        Value = r.recordId.ToString(),
                        Text = $"{r.patientName} - {r.recordDate:yyyy-MM-dd}" ?? "Unknown Record"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                MedicalRecordOptions = new List<SelectListItem>();
            }
        }
    }
}