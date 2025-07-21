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
        public string? Dosage { get; set; }

        [BindProperty(SupportsGet = true, Name = "page")]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public SelectList Medicines { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Records { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Lấy danh sách đơn thuốc có tìm kiếm và phân trang
            var query = $"?recordId={RecordId}&medicineId={MedicineId}&dosage={Dosage}" +
                        $"&page={CurrentPage}&pageSize={PageSize}";

            var prescriptionResponse = await client.GetAsync("https://localhost:7086/api/Prescription/search" + query);

            if (prescriptionResponse.IsSuccessStatusCode)
            {
                var json = await prescriptionResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                Prescriptions = JsonSerializer.Deserialize<List<PrescriptionViewModel>>(
                    doc.RootElement.GetProperty("data").GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new List<PrescriptionViewModel>();

                TotalRecords = doc.RootElement.GetProperty("totalRecords").GetInt32();
            }
            else
            {
                Prescriptions = new List<PrescriptionViewModel>();
                TotalRecords = 0;
            }

            // 2. Lấy danh sách thuốc (dropdown)
            var medicineResponse = await client.GetAsync("https://localhost:7086/api/Medicine");
            if (medicineResponse.IsSuccessStatusCode)
            {
                var json = await medicineResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var list = doc.RootElement.EnumerateArray()
                    .Select(item => new SelectListItem
                    {
                        Value = item.GetProperty("medicineId").GetInt32().ToString(),
                        Text = item.GetProperty("medicineName").GetString() ?? "Unknown"
                    }).ToList();

                Medicines = new SelectList(list, "Value", "Text");
            }

            // 3. Lấy danh sách hồ sơ (dropdown)
            var recordResponse = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
            if (recordResponse.IsSuccessStatusCode)
            {
                var json = await recordResponse.Content.ReadAsStringAsync();
                var records = JsonSerializer.Deserialize<List<MedicalRecordVM>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<MedicalRecordVM>();

                Records = new SelectList(records, "RecordId", "RecordId");
            }

            return Page();
        }
    }
}
