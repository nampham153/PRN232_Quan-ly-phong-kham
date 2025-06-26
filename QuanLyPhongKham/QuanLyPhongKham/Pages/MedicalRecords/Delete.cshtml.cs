using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyPhongKham.Pages.MedicalRecords
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
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
            {
                // Không tìm th?y ho?c l?i thì chuy?n v? Index
                return RedirectToPage("Index");
            }

            var json = await response.Content.ReadAsStringAsync();
            MedicalRecord = JsonSerializer.Deserialize<MedicalRecordVM>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new MedicalRecordVM();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.DeleteAsync($"https://localhost:7086/api/MedicalRecord/{MedicalRecord.RecordId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"L?i xóa h? s?: {error}");
            return Page();
        }
    }
}
