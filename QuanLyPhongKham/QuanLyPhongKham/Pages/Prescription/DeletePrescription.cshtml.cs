using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Prescription
{
    public class DeletePrescriptionModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeletePrescriptionModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public PrescriptionViewModel Prescription { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7086/api/Prescription/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            Prescription = JsonSerializer.Deserialize<PrescriptionViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7086/api/Prescription/{Prescription.PrescriptionId}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Xoá đơn thuốc thất bại.");
                return Page();
            }

            return RedirectToPage("ListPrescription");
        }
    }
}
