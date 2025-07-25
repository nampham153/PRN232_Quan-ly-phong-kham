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
        public PrescriptionViewModel Prescription { get; set; } = new();

        public SelectList Medicines { get; set; }
        public SelectList Records { get; set; }

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
                try
                {
                    var medicineResponse = await client.GetAsync("https://localhost:7086/api/Medicine");
                    if (medicineResponse.IsSuccessStatusCode)
                    {
                        var json = await medicineResponse.Content.ReadAsStringAsync();
                        var doc = JsonDocument.Parse(json);
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
                        Console.WriteLine($"Lỗi lấy danh sách thuốc: {(int)medicineResponse.StatusCode} {medicineResponse.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception khi gọi API thuốc: {ex.Message}");
                }

                // Load Records
                try
                {
                    var recordResponse = await client.GetAsync("https://localhost:7086/api/MedicalRecord");
                    if (recordResponse.IsSuccessStatusCode)
                    {
                        var json = await recordResponse.Content.ReadAsStringAsync();
                        var doc = JsonDocument.Parse(json);
                        var list = new List<SelectListItem>();

                        foreach (var item in doc.RootElement.EnumerateArray())
                        {
                            var id = item.GetProperty("recordId").GetInt32();

                            list.Add(new SelectListItem
                            {
                                Value = id.ToString(),
                                Text = $"Hồ sơ #{id}",
                                Selected = Prescription?.RecordId == id
                            });
                        }

                        Records = new SelectList(list, "Value", "Text");
                    }
                    else
                    {
                        Console.WriteLine($"Lỗi lấy danh sách hồ sơ: {(int)recordResponse.StatusCode} {recordResponse.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception khi gọi API hồ sơ: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi chung trong LoadDropdownsAsync: {ex.Message}");
            }
        }

    }
}
