using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Patient
{
    public class EditPatientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditPatientModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PatientViewModel PatientViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:5001/api/Patient/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            PatientViewModel = JsonSerializer.Deserialize<PatientViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();

            var jsonString = JsonSerializer.Serialize(PatientViewModel, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7086/api/Patient/{PatientViewModel.PatientId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Patient/PatientList");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật bệnh nhân: " + errorContent);
                return Page();
            }
        }
    }
}
