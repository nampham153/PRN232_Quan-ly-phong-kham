using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class UpdatePrescriptionModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UpdatePrescriptionModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PrescriptionViewModel Prescription { get; set; }

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();

            // Lấy đơn thuốc cần sửa
            var prescriptionResponse = await client.GetAsync($"https://localhost:7086/api/Prescription/{id}");
            if (!prescriptionResponse.IsSuccessStatusCode)
                return NotFound();

            var prescriptionJson = await prescriptionResponse.Content.ReadAsStringAsync();
            Prescription = JsonSerializer.Deserialize<PrescriptionViewModel>(prescriptionJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Prescription.RecordId == null || Prescription.MedicineId == null)
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
            var response = await client.PutAsync($"https://localhost:7086/api/Prescription/{Prescription.PrescriptionId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("ListPrescription");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Cập nhật đơn thuốc thất bại. " + error);
            await LoadDropdownsAsync();
            return Page();
        }


        private async Task LoadDropdownsAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Load Medicines (sử dụng JsonDocument để duyệt từng phần tử)
            var medicineResponse = await client.GetAsync("https://localhost:7086/api/Medicine");
            if (medicineResponse.IsSuccessStatusCode)
            {
                var json = await medicineResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var list = new List<SelectListItem>();

                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    var id = item.GetProperty("medicineId").GetInt32();
                    var name = item.GetProperty("medicineName").GetString();

                    list.Add(new SelectListItem
                    {
                        Value = id.ToString(),
                        Text = name,
                        Selected = Prescription?.MedicineId == id
                    });
                }

                Medicines = new SelectList(list, "Value", "Text");
            }
            else
            {
                Medicines = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Không thể tải thuốc --" }
        }, "Value", "Text");
            }

            // Load Records (giữ nguyên cách cũ nếu bạn không muốn đổi)
            var recordResponse = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
            if (recordResponse.IsSuccessStatusCode)
            {
                var json = await recordResponse.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<MedicalRecordVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Records = new SelectList(list, "RecordId", "RecordId", Prescription?.RecordId);
            }
            else
            {
                Records = new SelectList(new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Không thể tải hồ sơ --" }
        }, "Value", "Text");
            }
        }

    }
}
