using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using DataAccessLayer.ViewModels;

namespace QuanLyPhongKham.Pages.Patient
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7086/api/");
        }

        [BindProperty]
        public PatientViewModel Patient { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Patient/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Không tìm thấy bệnh nhân.";
                return RedirectToPage("PatientList");
            }

            var content = await response.Content.ReadAsStringAsync();
            Patient = JsonSerializer.Deserialize<PatientViewModel>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Patient == null || Patient.PatientId == 0)
            {
                ErrorMessage = "Bệnh nhân không hợp lệ.";
                return RedirectToPage("/Patient/PatientList");
            }

            var response = await _httpClient.DeleteAsync($"Patient/{Patient.PatientId}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Patient/PatientList");
            }
            else
            {
                ErrorMessage = "Không thể xóa bệnh nhân vì có liên kết đến hồ sơ y tế.";
                return RedirectToPage("/Patient/DeletePatient", new { id = Patient.PatientId });
            }
        }


    }
}
