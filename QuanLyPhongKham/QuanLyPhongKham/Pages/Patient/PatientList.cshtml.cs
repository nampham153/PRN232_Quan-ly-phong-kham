using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Patient
{
    public class PatientList : PageModel
    {
        private readonly HttpClient _httpClient;

        public PatientList(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7086/api/"); // API base address
        }

        public List<PatientViewModel> Patients { get; set; } = new List<PatientViewModel>();

        [BindProperty(SupportsGet = true)]
        public string SearchFullName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchPhone { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchEmail { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchAddress { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchAll { get; set; }

        [BindProperty(SupportsGet = true)]
        public string GenderFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DOBFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DOBTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchUnderlyingDiseases { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchMedicalInfo { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchAll))
            {
                SearchFullName = SearchAll;
                SearchPhone = SearchAll;
                SearchEmail = SearchAll;
                SearchAddress = SearchAll;
                SearchUnderlyingDiseases = SearchAll;
                SearchMedicalInfo = SearchAll;
            }

            var queryString = BuildQueryString();
            var response = await _httpClient.GetAsync($"Patient/search?{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var patients = JsonSerializer.Deserialize<List<PatientViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Patients = patients;
            }
            else
            {
                Patients = new List<PatientViewModel>();
            }
        }

        private string BuildQueryString()
        {
            var parameters = new List<string>();

            if (!string.IsNullOrEmpty(SearchFullName)) parameters.Add($"fullName={Uri.EscapeDataString(SearchFullName)}");
            if (!string.IsNullOrEmpty(SearchPhone)) parameters.Add($"phone={Uri.EscapeDataString(SearchPhone)}");
            if (!string.IsNullOrEmpty(SearchEmail)) parameters.Add($"email={Uri.EscapeDataString(SearchEmail)}");
            if (!string.IsNullOrEmpty(SearchAddress)) parameters.Add($"address={Uri.EscapeDataString(SearchAddress)}");
            if (!string.IsNullOrEmpty(GenderFilter)) parameters.Add($"gender={Uri.EscapeDataString(GenderFilter)}");
            if (DOBFrom.HasValue) parameters.Add($"dobFrom={DOBFrom.Value:yyyy-MM-dd}");
            if (DOBTo.HasValue) parameters.Add($"dobTo={DOBTo.Value:yyyy-MM-dd}");
            if (!string.IsNullOrEmpty(SearchUnderlyingDiseases)) parameters.Add($"underlyingDiseases={Uri.EscapeDataString(SearchUnderlyingDiseases)}");
            if (!string.IsNullOrEmpty(SearchMedicalInfo)) parameters.Add($"medicalInfo={Uri.EscapeDataString(SearchMedicalInfo)}");

            return string.Join("&", parameters);
        }
    }
}
