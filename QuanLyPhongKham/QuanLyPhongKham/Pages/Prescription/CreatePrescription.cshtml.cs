using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class CreatePrescriptionModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreatePrescriptionModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PrescriptionViewModel Prescription { get; set; } = new()
        {
            Date = DateTime.Now // Gán giá trị mặc định
        };

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }
        public string Diagnosis { get; set; } // Để hiển thị Diagnosis
        public string DoctorName { get; set; } // Để hiển thị DoctorName

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(Prescription, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7086/api/Prescription", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Prescription created successfully.";
                return RedirectToPage("ListPrescription");
            }

            ModelState.AddModelError(string.Empty, "Không thể thêm đơn thuốc.");
            await LoadDropdownsAsync();
            return Page();
        }

        private async Task LoadDropdownsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Load Medicines
                var medicineResponse = await client.GetAsync("https://localhost:7086/api/Medicine");
                if (medicineResponse.IsSuccessStatusCode)
                {
                    var json = await medicineResponse.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    var list = doc.RootElement.EnumerateArray()
                        .Select(item => new SelectListItem
                        {
                            Value = item.GetProperty("medicineId").GetInt32().ToString(),
                            Text = item.GetProperty("medicineName").GetString() ?? "Unknown",
                            Selected = Prescription?.MedicineId == item.GetProperty("medicineId").GetInt32()
                        }).ToList();
                    Medicines = new SelectList(list, "Value", "Text");
                }
                else
                {
                    Console.WriteLine($"Lỗi lấy danh sách thuốc: {(int)medicineResponse.StatusCode} {medicineResponse.ReasonPhrase}");
                    Medicines = new SelectList(Enumerable.Empty<SelectListItem>());
                }

                // Load Records
                var recordResponse = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
                if (recordResponse.IsSuccessStatusCode)
                {
                    var json = await recordResponse.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(json);
                    var list = doc.RootElement.EnumerateArray()
                        .Select(item => new SelectListItem
                        {
                            Value = item.GetProperty("recordId").GetInt32().ToString(),
                            Text = $"Hồ sơ #{item.GetProperty("recordId").GetInt32()}",
                            Selected = Prescription?.RecordId == item.GetProperty("recordId").GetInt32()
                        }).ToList();
                    Records = new SelectList(list, "Value", "Text");

                    // Nếu có RecordId được chọn, tải Diagnosis và DoctorName
                    if (Prescription.RecordId.HasValue)
                    {
                        var recordDetailResponse = await client.GetAsync($"https://localhost:7086/api/MedicalRecord/{Prescription.RecordId}");
                        if (recordDetailResponse.IsSuccessStatusCode)
                        {
                            var recordJson = await recordDetailResponse.Content.ReadAsStringAsync();
                            var recordDoc = JsonDocument.Parse(recordJson);
                            Diagnosis = recordDoc.RootElement.GetProperty("diagnosis").GetString() ?? "N/A";
                            var userId = recordDoc.RootElement.GetProperty("userId").GetInt32();
                            var userResponse = await client.GetAsync($"https://localhost:7086/api/User/{userId}");
                            if (userResponse.IsSuccessStatusCode)
                            {
                                var userJson = await userResponse.Content.ReadAsStringAsync();
                                var userDoc = JsonDocument.Parse(userJson);
                                DoctorName = userDoc.RootElement.GetProperty("fullName").GetString() ?? "N/A";
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Lỗi lấy danh sách hồ sơ: {(int)recordResponse.StatusCode} {recordResponse.ReasonPhrase}");
                    Records = new SelectList(Enumerable.Empty<SelectListItem>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi chung trong LoadDropdownsAsync: {ex.Message}");
                Medicines = new SelectList(Enumerable.Empty<SelectListItem>());
                Records = new SelectList(Enumerable.Empty<SelectListItem>());
            }
        }
    }
}