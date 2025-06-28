using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class ListPrescriptionModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ListPrescriptionModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<PrescriptionViewModel> Prescriptions { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? RecordId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MedicineId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Dosage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Quantity { get; set; }

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // Gọi API tìm đơn thuốc
            var searchUrl = $"https://localhost:7086/api/Prescription/search?recordId={RecordId}&medicineId={MedicineId}&quantity={Quantity}&dosage={Dosage}";
            var prescriptionResponse = await client.GetAsync(searchUrl);

            if (prescriptionResponse.IsSuccessStatusCode)
            {
                var json = await prescriptionResponse.Content.ReadAsStringAsync();
                Prescriptions = JsonSerializer.Deserialize<List<PrescriptionViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            // Gọi API để lấy danh sách thuốc
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

            // Gọi API để lấy danh sách hồ sơ
            var recordResponse = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
            if (recordResponse.IsSuccessStatusCode)
            {
                var json = await recordResponse.Content.ReadAsStringAsync();
                var records = JsonSerializer.Deserialize<List<MedicalRecordVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                Records = new SelectList(records, "RecordId", "RecordId"); // hoặc "PatientName" nếu muốn
            }

            return Page();
        }
    }
}
