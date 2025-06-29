using DataAccessLayer.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using DataAccessLayer.ViewModels.Authen;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace QuanLyPhongKham.Pages.Authen
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public CreateAccountDto Account { get; set; }

        public List<SelectListItem> RoleSelectList { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadRolesAsync();

            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(Account.Password))
            {
                ModelState.AddModelError("Account.Password", "Mật khẩu không được để trống.");
                return Page();
            }

            var selectedRole = RoleSelectList.FirstOrDefault(r => r.Value == Account.RoleId.ToString());
            if (selectedRole == null)
            {
                ModelState.AddModelError("Account.RoleId", "Vai trò không hợp lệ.");
                return Page();
            }

            var dto = new AccountCreateDto
            {
                AccountId = 0,
                Username = Account.Username,
                RoleId = Account.RoleId,
                RoleName = selectedRole.Text,
                FullName = Account.FullName,
                Status = Account.Status,
                Password = Account.Password
            };

            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://localhost:7086/api/account", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Không thể tạo tài khoản: {msg}");
                return Page();
            }

            return RedirectToPage("/Authen/Index");
        }


        private async Task LoadRolesAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return; // hoặc RedirectToPage("/Authen/Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7086/api/account/roles");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var roles = JsonSerializer.Deserialize<List<Role>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new();

                RoleSelectList = roles.Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                }).ToList();
            }
        }


        public class CreateAccountDto
        {
            public int AccountId { get; set; } = 0;

            [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Họ và tên không được để trống")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Vui lòng chọn vai trò")]
            [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vai trò hợp lệ")]
            public int RoleId { get; set; }

            public bool Status { get; set; } = false;
        }
    }
}
