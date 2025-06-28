using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyPhongKham.Pages.Doctors
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7086/api/doctor";

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public DoctorVM Doctor { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Doctor = await _httpClient.GetFromJsonAsync<DoctorVM>($"{_apiBaseUrl}/{id}");
            if (Doctor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!Doctor.DOB.HasValue)
            {
                ModelState.AddModelError("Doctor.DOB", "Vui lòng nhập ngày sinh.");
                return Page();
            }

            var today = DateTime.Today;
            var dob = Doctor.DOB.Value;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            if (age < 18)
            {
                ModelState.AddModelError("Doctor.DOB", "Bác sĩ phải từ 18 tuổi trở lên.");
                return Page();
            }

            // Gửi PUT request cập nhật doctor
            var response = await _httpClient.PutAsJsonAsync($"{_apiBaseUrl}/{Doctor.AccountId}", Doctor);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Lỗi khi cập nhật bác sĩ: " + error);
                return Page();
            }
        }
    }
}
