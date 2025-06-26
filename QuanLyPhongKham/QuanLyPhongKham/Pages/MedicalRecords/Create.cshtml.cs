using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLayer.ViewModels;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyPhongKham.Pages.MedicalRecords
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7086/api/MedicalRecord";

        public CreateModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public MedicalRecordVM MedicalRecord { get; set; } = new MedicalRecordVM();

        public IActionResult OnGet()
        {
            MedicalRecord.Date = System.DateTime.Today;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var jsonString = JsonSerializer.Serialize(MedicalRecord, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiBaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    var errorJson = await response.Content.ReadAsStringAsync();

                    try
                    {
                        using var doc = JsonDocument.Parse(errorJson);
                        if (doc.RootElement.TryGetProperty("message", out var messageProp))
                        {
                            var message = messageProp.GetString();
                            ModelState.AddModelError(string.Empty, message ?? "Có l?i x?y ra khi t?o h? s?.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, errorJson);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError(string.Empty, errorJson);
                    }

                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "?ã x?y ra l?i: " + ex.Message);
                return Page();
            }
        }
    }
}
