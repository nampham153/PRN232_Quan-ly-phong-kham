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
        public IFormFile AvatarFile { get; set; }

        [BindProperty]
        public PatientViewModel PatientViewModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7086/api/Patient/{id}");

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

            // Nếu người dùng upload ảnh mới
            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot/uploadsPatient");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn ảnh mới
                PatientViewModel.AvatarPath = $"/uploadsPatient/{fileName}";
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
