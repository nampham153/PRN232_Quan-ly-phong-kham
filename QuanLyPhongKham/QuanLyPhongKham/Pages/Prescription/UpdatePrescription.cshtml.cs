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
            var prescriptionResponse = await client.GetAsync($"https://localhost:5001/api/Prescription/{id}");
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

            var response = await client.PutAsync($"https://localhost:7086/api/Prescription/{Prescription.PrescriptionId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("ListPrescription");
            }

            ModelState.AddModelError(string.Empty, "Cập nhật đơn thuốc thất bại.");
            await LoadDropdownsAsync();
            return Page();
        }

        private async Task LoadDropdownsAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Thuốc
            var medicineRes = await client.GetAsync("https://localhost:7086/api/Medicine");
            if (medicineRes.IsSuccessStatusCode)
            {
                var json = await medicineRes.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<MedicineVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Medicines = new SelectList(list, "MedicineId", "MedicineName");
            }

            // Hồ sơ bệnh án
            var recordRes = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
            if (recordRes.IsSuccessStatusCode)
            {
                var json = await recordRes.Content.ReadAsStringAsync();
                var list = JsonSerializer.Deserialize<List<MedicalRecordVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Records = new SelectList(list, "RecordId", "RecordId");
            }
        }
    }
}
