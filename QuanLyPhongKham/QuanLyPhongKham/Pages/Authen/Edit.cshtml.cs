using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace QuanLyPhongKham.Pages.Authen
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public AccountUpdateDto Account { get; set; }

        public List<SelectListItem> RoleSelectList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"https://localhost:7086/api/account/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var accountData = JsonSerializer.Deserialize<AccountUpdateDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (accountData == null)
                return NotFound();

            Account = accountData;
            Account.Password = ""; // Reset để trống mật khẩu
            Console.WriteLine($"AccountId: {accountData.AccountId}");
            Console.WriteLine($"Username: {accountData.Username}");
            Console.WriteLine($"FullName: {accountData.FullName}");
            Console.WriteLine($"RoleId: {accountData.RoleId}");
            Console.WriteLine($"Status: {accountData.Status}");

            await LoadRolesAsync(token);
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            await LoadRolesAsync(token);

            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(Account.Password))
            {
                ModelState.AddModelError("Account.Password", "Mật khẩu không được để trống.");
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(Account),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PutAsync("https://localhost:7086/api/account", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Cập nhật tài khoản thất bại: {msg}");
                return Page();
            }

            return RedirectToPage("/Authen/Index");
        }

        private async Task LoadRolesAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7086/api/account/roles");
            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            var roles = JsonSerializer.Deserialize<List<RoleDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            RoleSelectList = roles?.Select(r => new SelectListItem
            {
                Value = r.RoleId.ToString(),
                Text = r.RoleName
            }).ToList() ?? new();
        }

        // ✅ ViewModel đúng theo API
        public class AccountUpdateDto
        {
            public int AccountId { get; set; }
            public string Username { get; set; } = string.Empty;
            public int RoleId { get; set; }
            public string FullName { get; set; } = string.Empty;
            public bool Status { get; set; }
            public string Password { get; set; } = string.Empty;
        }

        public class RoleDto
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
        }
    }
}
