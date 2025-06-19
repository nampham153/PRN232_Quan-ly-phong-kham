using DataAccessLayer.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Authen
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? RoleId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? Status { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        public List<Account> Accounts { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy token từ Session
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
            {
                // Chưa đăng nhập, chuyển về login
                return RedirectToPage("/Authen/Login");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Lấy danh sách roles (cũng có thể cache nếu cần)
            var rolesResponse = await client.GetAsync("https://localhost:7086/api/account/roles");
            if (rolesResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Token hết hạn hoặc không hợp lệ
                return RedirectToPage("/Authen/Login");
            }

            if (rolesResponse.IsSuccessStatusCode)
            {
                var rolesJson = await rolesResponse.Content.ReadAsStringAsync();
                Roles = JsonSerializer.Deserialize<List<Role>>(rolesJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            }

            // Tạo query string cho count
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(Search))
                queryParams.Add($"search={Uri.EscapeDataString(Search)}");


            if (RoleId.HasValue)
                queryParams.Add($"roleId={RoleId.Value}");
            if (Status.HasValue)
                queryParams.Add($"status={Status.Value}");

            var countQuery = string.Join("&", queryParams);
            var countUrl = "https://localhost:7086/api/account/count";
            if (!string.IsNullOrEmpty(countQuery))
                countUrl += "?" + countQuery;

            var countResponse = await client.GetAsync(countUrl);
            if (countResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/Authen/Login");
            }

            if (countResponse.IsSuccessStatusCode)
            {
                var countJson = await countResponse.Content.ReadAsStringAsync();
                if (int.TryParse(countJson, out int totalCount))
                {
                    TotalPages = (int)Math.Ceiling(totalCount / 6.0);
                }
            }

            if (Page < 1) Page = 1;
            if (Page > TotalPages) Page = TotalPages;

            queryParams.Add($"page={Page}");
            var listQuery = string.Join("&", queryParams);
            var listUrl = "https://localhost:7086/api/account/list";
            if (!string.IsNullOrEmpty(listQuery))
                listUrl += "?" + listQuery;

            var listResponse = await client.GetAsync(listUrl);
            if (listResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/Authen/Login");
            }

            if (listResponse.IsSuccessStatusCode)
            {
                var listJson = await listResponse.Content.ReadAsStringAsync();
                Accounts = JsonSerializer.Deserialize<List<Account>>(listJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            }

            return Page();
        }
    }
}
