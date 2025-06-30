using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Doctors
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public DoctorVM Doctor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7086/api/doctor/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            Doctor = JsonSerializer.Deserialize<DoctorVM>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Doctor == null || Doctor.AccountId == 0)
                return BadRequest();

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7086/api/doctor/{Doctor.AccountId}");

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"L?i API khi xóa: {error}");
            return Page();
        }
    }
}
