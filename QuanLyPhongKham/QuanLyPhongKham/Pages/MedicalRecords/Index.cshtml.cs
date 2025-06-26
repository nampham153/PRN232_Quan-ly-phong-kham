using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using DataAccessLayer.models;
using Microsoft.Extensions.Configuration;

namespace QuanLyPhongKham.Pages.MedicalRecords
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;

        public IndexModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7086";
        }

        public List<MedicalRecordVM> MedicalRecords { get; set; } = new List<MedicalRecordVM>();
        public List<SelectListItem> DoctorOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> PatientOptions { get; set; } = new List<SelectListItem>();

        [BindProperty(SupportsGet = true)]
        public int? SelectedDoctorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedPatientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        // Wrapper ?? deserialize JSON ki?u { data: [...], totalRecords: ... }
        public class PagedResponse<T>
        {
            [JsonProperty("data")]
            public List<T> Data { get; set; } = new List<T>();

            [JsonProperty("totalRecords")]
            public int TotalRecords { get; set; }

            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("pageSize")]
            public int PageSize { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDoctorOptions();
            await LoadPatientOptions();

            var queryParams = new List<string>();
            if (SelectedDoctorId.HasValue)
                queryParams.Add($"userId={SelectedDoctorId.Value}");
            if (SelectedPatientId.HasValue)
                queryParams.Add($"patientId={SelectedPatientId.Value}");
            if (!string.IsNullOrEmpty(SearchTerm))
                queryParams.Add($"searchTerm={Uri.EscapeDataString(SearchTerm)}");

            var apiUrl = $"{_apiBaseUrl}/api/MedicalRecord/filter";
            if (queryParams.Count > 0)
                apiUrl += "?" + string.Join("&", queryParams);

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize JSON d?ng { data: [...], totalRecords: ..., ... }
                var pagedResult = JsonConvert.DeserializeObject<PagedResponse<MedicalRecordVM>>(content);

                MedicalRecords = pagedResult?.Data ?? new List<MedicalRecordVM>();
            }
            else
            {
                MedicalRecords = new List<MedicalRecordVM>();
                TempData["ErrorMessage"] = "Failed to load medical records.";
            }

            return Page();
        }

        private async Task LoadDoctorOptions()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Doctor");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var doctorsWrapper = JsonConvert.DeserializeObject<DoctorListResponse>(content);

                    DoctorOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "All Doctors", Selected = !SelectedDoctorId.HasValue }
                    };

                    if (doctorsWrapper?.Data != null)
                    {
                        DoctorOptions.AddRange(doctorsWrapper.Data.Select(d =>
                            new SelectListItem
                            {
                                Value = d.AccountId.ToString(),
                                Text = d.FullName,
                                Selected = SelectedDoctorId.HasValue && d.AccountId == SelectedDoctorId.Value
                            }));
                    }
                }
            }
            catch
            {
                DoctorOptions = new List<SelectListItem> { new SelectListItem { Value = "", Text = "All Doctors" } };
            }
        }

        private async Task LoadPatientOptions()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/Patient"); // Gi? s? API có endpoint này tr? list b?nh nhân
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var patientsWrapper = JsonConvert.DeserializeObject<PatientListResponse>(content);

                    PatientOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "All Patients", Selected = !SelectedPatientId.HasValue }
                    };

                    if (patientsWrapper?.Data != null)
                    {
                        PatientOptions.AddRange(patientsWrapper.Data.Select(p =>
                            new SelectListItem
                            {
                                Value = p.PatientId.ToString(),
                                Text = p.FullName,
                                Selected = SelectedPatientId.HasValue && p.PatientId == SelectedPatientId.Value
                            }));
                    }
                }
            }
            catch
            {
                PatientOptions = new List<SelectListItem> { new SelectListItem { Value = "", Text = "All Patients" } };
            }
        }

        public class DoctorListResponse
        {
            public List<DoctorVM> Data { get; set; } = new List<DoctorVM>();
            public int TotalRecords { get; set; }
        }

        public class PatientListResponse
        {
            public List<PatientViewModel> Data { get; set; } = new List<PatientViewModel>();
            public int TotalRecords { get; set; }
        }

        // X? lý xóa record
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/MedicalRecord/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Medical record deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete medical record.";
            }

            var routeValues = new Dictionary<string, object>();
            if (SelectedDoctorId.HasValue) routeValues["SelectedDoctorId"] = SelectedDoctorId.Value;
            if (SelectedPatientId.HasValue) routeValues["SelectedPatientId"] = SelectedPatientId.Value;
            if (!string.IsNullOrEmpty(SearchTerm)) routeValues["SearchTerm"] = SearchTerm;

            return RedirectToPage(routeValues);
        }
    }
}
