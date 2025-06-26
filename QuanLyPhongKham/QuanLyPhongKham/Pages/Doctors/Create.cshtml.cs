using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using DataAccessLayer.models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Azure;
namespace QuanLyPhongKham.Pages.Doctors
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public DoctorVM Doctor { get; set; } = new();

        public List<Account> AvailableAccounts { get; set; } = new();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;

        public CreateModel(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAvailableAccountsAsync();
            return Page();
        }

        private async Task LoadAvailableAccountsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7086/api/doctor/accounts/available");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                AvailableAccounts = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Account>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAvailableAccountsAsync();
                return Page();
            }
            // Nếu có file ảnh upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploadsDoctor");
                Directory.CreateDirectory(uploadFolder); // Đảm bảo thư mục tồn tại

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Doctor.DoctorPath = "/uploadsDoctor/" + fileName;
            }

            var client = _httpClientFactory.CreateClient();

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(Doctor),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://localhost:7086/api/Doctor", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Lỗi tạo bác sĩ: " + error);

            await LoadAvailableAccountsAsync();
            return Page();
        }
    }
}
