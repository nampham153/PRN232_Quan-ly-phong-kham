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

        

        [BindProperty(SupportsGet = true, Name = "page")]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 11;

        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public SelectList Medicines { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());
        public SelectList Records { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public async Task<IActionResult> OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            // API phân trang + tìm kiếm
            var searchUrl = $"https://localhost:7086/api/Prescription/search" +
                $"?recordId={RecordId}&medicineId={MedicineId}&dosage={Dosage}" +
                $"&page={CurrentPage}&pageSize={PageSize}";

            var prescriptionResponse = await client.GetAsync(searchUrl);

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

            // Danh sách thuốc
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
                        Text = name
                    });
                }

                Medicines = new SelectList(list, "Value", "Text");

            }

            // Danh sách hồ sơ
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
