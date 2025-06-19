using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyPhongKham.Pages.Doctors
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7086/api/doctor";

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<DoctorVM> Doctors { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiBaseUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var allDoctors = JsonSerializer.Deserialize<List<DoctorVM>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (!string.IsNullOrEmpty(SearchTerm))
                    {
                        allDoctors = allDoctors
                            .FindAll(d => !string.IsNullOrEmpty(d.FullName) && d.FullName.ToLower().Contains(SearchTerm.ToLower()));
                    }

                    Doctors = allDoctors;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Không thể load danh sách bác sĩ từ API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi gọi API: {ex.Message}");
            }
        }
    }
}
