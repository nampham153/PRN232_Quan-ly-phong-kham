using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
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
        public PrescriptionViewModel Prescription { get; set; }

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Lấy danh sách thuốc
            var medicineResponse = await client.GetAsync("https://localhost:7086/api/Medicine");
            if (medicineResponse.IsSuccessStatusCode)
            {
                var json = await medicineResponse.Content.ReadAsStringAsync();
                var medicines = JsonSerializer.Deserialize<List<MedicineVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Medicines = new SelectList(medicines, "MedicineId", "MedicineName");
            }

            // Lấy danh sách hồ sơ bệnh án
            var recordResponse = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
            if (recordResponse.IsSuccessStatusCode)
            {
                var json = await recordResponse.Content.ReadAsStringAsync();
                var records = JsonSerializer.Deserialize<List<MedicalRecordVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Records = new SelectList(records, "RecordId", "RecordId");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Reload dropdown nếu bị lỗi
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
                return RedirectToPage("ListPrescription");
            }

            ModelState.AddModelError(string.Empty, "Lỗi khi thêm đơn thuốc.");
            await OnGetAsync(); // Reload dropdown
            return Page();
        }
    }
}
