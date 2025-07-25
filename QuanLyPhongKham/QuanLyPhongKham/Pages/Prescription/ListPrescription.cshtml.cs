using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Text.Json;
using DataAccessLayer.models;

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

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; } // Ngày bắt đầu

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; } // Ngày kết thúc

        [BindProperty(SupportsGet = true)]
        public string? Diagnosis { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DoctorName { get; set; }

        [BindProperty(SupportsGet = true, Name = "page")]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public SelectList Medicines { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Records { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Doctors { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Lấy danh sách đơn thuốc có tìm kiếm và phân trang
            var queryParams = new List<string>();
            if (RecordId.HasValue) queryParams.Add($"recordId={RecordId.Value}");
            if (MedicineId.HasValue) queryParams.Add($"medicineId={MedicineId.Value}");
            if (!string.IsNullOrEmpty(Dosage)) queryParams.Add($"dosage={Uri.EscapeDataString(Dosage)}");
            if (StartDate.HasValue) queryParams.Add($"startDate={StartDate.Value:yyyy-MM-dd}");
            if (EndDate.HasValue) queryParams.Add($"endDate={EndDate.Value:yyyy-MM-dd}");
            if (!string.IsNullOrEmpty(Diagnosis)) queryParams.Add($"diagnosis={Uri.EscapeDataString(Diagnosis)}");
            if (!string.IsNullOrEmpty(DoctorName)) queryParams.Add($"doctorName={Uri.EscapeDataString(DoctorName)}");
            queryParams.Add($"page={CurrentPage}");
            queryParams.Add($"pageSize={PageSize}");
            var query = string.Join("&", queryParams);

            var prescriptionResponse = await client.GetAsync($"https://localhost:7086/api/Prescription/search?{query}");

            if (prescriptionResponse.IsSuccessStatusCode)
            {
                var json = await prescriptionResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var rootElement = doc.RootElement;

                // Kiểm tra và lấy dữ liệu từ JSON
                if (rootElement.TryGetProperty("data", out var dataElement))
                {
                    Prescriptions = JsonSerializer.Deserialize<List<PrescriptionViewModel>>(
                        dataElement.GetRawText(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    ) ?? new List<PrescriptionViewModel>();
                }
                else
                {
                    Prescriptions = new List<PrescriptionViewModel>();
                }

                if (rootElement.TryGetProperty("totalRecords", out var totalRecordsElement))
                {
                    TotalRecords = totalRecordsElement.GetInt32();
                }
                else
                {
                    TotalRecords = 0;
                }
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