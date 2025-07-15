using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongKham.Pages.MedicalRecords
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public MedicalRecordVM MedicalRecord { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7086/api/MedicalRecord/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            MedicalRecord = JsonSerializer.Deserialize<MedicalRecordVM>(json, options) ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient();

            // Chu?n b? JSON bao g?m c? tr??ng Status
            var json = JsonSerializer.Serialize(MedicalRecord, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7086/api/MedicalRecord/{MedicalRecord.RecordId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"L?i c?p nh?t: {error}");
            return Page();
        }
    }
}
