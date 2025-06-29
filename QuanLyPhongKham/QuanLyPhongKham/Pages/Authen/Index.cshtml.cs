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

        // ✅ Đổi tên không bị conflict với từ khóa hệ thống
        [BindProperty(SupportsGet = true)]
        public int Pages { get; set; } = 1;

        public List<AccountDTO> Accounts { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
        public int TotalPages { get; set; }
        public int CurrentPage => Pages;

        public async Task<IActionResult> OnGetAsync(
      string? search,
      int? roleId,
      bool? status,
      int pages = 1)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Gán giá trị từ query vào property
            Search = search;
            RoleId = roleId;
            Status = status;
            Pages = pages;

            // 1. Lấy danh sách Role
            var roleResponse = await client.GetAsync("https://localhost:7086/api/account/roles");
            if (roleResponse.IsSuccessStatusCode)
            {
                var rolesJson = await roleResponse.Content.ReadAsStringAsync();
                Roles = JsonSerializer.Deserialize<List<Role>>(rolesJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            }

            // 2. Tạo query filter
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(Search))
                queryParams.Add($"search={Uri.EscapeDataString(Search)}");
            if (RoleId.HasValue)
                queryParams.Add($"roleId={RoleId.Value}");
            if (Status.HasValue)
                queryParams.Add($"status={Status.Value}");

            // 3. Gọi API đếm tổng số tài khoản
            var countUrl = "https://localhost:7086/api/account/count";
            if (queryParams.Any())
                countUrl += "?" + string.Join("&", queryParams);

            var countResponse = await client.GetAsync(countUrl);
            if (countResponse.IsSuccessStatusCode)
            {
                var countJson = await countResponse.Content.ReadAsStringAsync();
                if (int.TryParse(countJson, out int totalCount))
                {
                    TotalPages = (int)Math.Ceiling(totalCount / 6.0);
                }
            }

            if (TotalPages <= 0) TotalPages = 1;
            Pages = Math.Clamp(Pages, 1, TotalPages); // Cố định trong khoảng hợp lệ

            // 4. Gọi API danh sách
            queryParams.Add($"page={Pages}");
            var listUrl = "https://localhost:7086/api/account/list?" + string.Join("&", queryParams);

            Console.WriteLine($"[DEBUG] GET list URL: {listUrl}");

            var listResponse = await client.GetAsync(listUrl);
            if (listResponse.IsSuccessStatusCode)
            {
                var listJson = await listResponse.Content.ReadAsStringAsync();
                Accounts = JsonSerializer.Deserialize<List<AccountDTO>>(listJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();
            }

            // DEBUG
            Console.WriteLine($"[DEBUG] Page: {Pages}");
            Console.WriteLine($"[DEBUG] Search: {Search}");
            Console.WriteLine($"[DEBUG] RoleId: {RoleId}");
            Console.WriteLine($"[DEBUG] Status: {Status}");

            return Page();
        }


    }

    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
