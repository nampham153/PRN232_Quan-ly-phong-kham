﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.ViewModels;
using System.Text.Json;
using System.Text;

namespace QuanLyPhongKham.Pages.Patient
{
    public class CreatePatientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreatePatientModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PatientViewModel PatientViewModel { get; set; }

        [BindProperty]
        public IFormFile AvatarFile { get; set; }

        public void OnGet()
        {
            PatientViewModel = new PatientViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // BẮT BUỘC PHẢI CÓ FILE
            if (AvatarFile == null || AvatarFile.Length == 0)
            {
                ModelState.AddModelError("AvatarFile", "Bạn phải chọn ảnh từ máy.");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Upload ảnh
                var uploadsFolder = Path.Combine("wwwroot/uploadsPatient");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(AvatarFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                // Gán đường dẫn ảnh vào ViewModel
                PatientViewModel.AvatarPath = $"/uploadsPatient/{fileName}";

                // Gọi API để tạo bệnh nhân
                var client = _httpClientFactory.CreateClient();
                var jsonString = JsonSerializer.Serialize(PatientViewModel, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:7086/api/Patient", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Patient/PatientList");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "Lỗi khi tạo bệnh nhân: " + errorContent);
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống: " + ex.Message);
                return Page();
            }
        }
    }
}
