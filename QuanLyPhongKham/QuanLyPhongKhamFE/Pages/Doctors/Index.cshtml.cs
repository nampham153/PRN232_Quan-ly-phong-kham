using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuanLyPhongKham.Pages.Doctors
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

        public List<DoctorVM> Doctors { get; set; } = new();
        public List<SelectListItem> NameOptions { get; set; } = new();
        public List<SelectListItem> EmailOptions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SelectedName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedEmail { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await LoadFilterOptions();

                var query = new List<string>();
                if (!string.IsNullOrEmpty(SelectedName))
                    query.Add($"name={Uri.EscapeDataString(SelectedName)}");
                if (!string.IsNullOrEmpty(SelectedEmail))
                    query.Add($"email={Uri.EscapeDataString(SelectedEmail)}");

                var apiUrl = $"{_apiBaseUrl}/api/Doctor";
                if (query.Any())
                    apiUrl += "?" + string.Join("&", query);

                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["ErrorMessage"] = "Failed to fetch doctors.";
                    return Page();
                }

                var json = await response.Content.ReadAsStringAsync();
                var wrapper = JsonConvert.DeserializeObject<DoctorResponseWrapper>(json);
                Doctors = wrapper?.Data ?? new();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading doctor list: {ex.Message}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/Doctor/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Doctor deleted successfully.";
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var error = JObject.Parse(content);
                        TempData["ErrorMessage"] = error["message"]?.ToString() ?? "Failed to delete doctor.";
                    }
                    catch
                    {
                        TempData["ErrorMessage"] = "Failed to delete doctor.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting doctor: {ex.Message}";
            }

            var routeValues = new RouteValueDictionary();
            if (!string.IsNullOrEmpty(SelectedName))
                routeValues["SelectedName"] = SelectedName;
            if (!string.IsNullOrEmpty(SelectedEmail))
                routeValues["SelectedEmail"] = SelectedEmail;

            return RedirectToPage(routeValues);
        }

        private async Task LoadFilterOptions()
        {
            // Load distinct names
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Doctor/names");
                var content = await response.Content.ReadAsStringAsync();
                var names = JsonConvert.DeserializeObject<List<string>>(content) ?? new();

                NameOptions = new List<SelectListItem> {
                    new SelectListItem { Value = "", Text = "All Names", Selected = string.IsNullOrEmpty(SelectedName) }
                };
                NameOptions.AddRange(names.Select(name => new SelectListItem
                {
                    Value = name,
                    Text = name,
                    Selected = name == SelectedName
                }));
            }
            catch
            {
                NameOptions = new List<SelectListItem> {
                    new SelectListItem { Value = "", Text = "All Names" }
                };
            }

            // Load distinct emails
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Doctor/emails");
                var content = await response.Content.ReadAsStringAsync();
                var emails = JsonConvert.DeserializeObject<List<string>>(content) ?? new();

                EmailOptions = new List<SelectListItem> {
                    new SelectListItem { Value = "", Text = "All Emails", Selected = string.IsNullOrEmpty(SelectedEmail) }
                };
                EmailOptions.AddRange(emails.Select(email => new SelectListItem
                {
                    Value = email,
                    Text = email,
                    Selected = email == SelectedEmail
                }));
            }
            catch
            {
                EmailOptions = new List<SelectListItem> {
                    new SelectListItem { Value = "", Text = "All Emails" }
                };
            }
        }

        private class DoctorResponseWrapper
        {
            public List<DoctorVM> Data { get; set; } = new();
            public int TotalRecords { get; set; }
        }
    }
}
