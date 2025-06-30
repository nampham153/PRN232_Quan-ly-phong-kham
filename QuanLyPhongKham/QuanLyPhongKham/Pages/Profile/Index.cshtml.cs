using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public UserDTO? User { get; set; }

        [BindProperty]
        public ChangePasswordViewModel PasswordInput { get; set; } = new();

        [BindProperty]
        public UserAccountViewModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7086/api/UserInfomation/infor");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<UserDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (User != null)
                {
                    // Mapping vào input
                    Input.AccountId = User.AccountId;
                    Input.FullName = User.FullName;
                    Input.Email = User.Email;
                    Input.Phone = User.Phone;
                    Input.DOB = User.DOB;
                    Input.Username = User.Username;
                    Input.RoleId = User.RoleId;
                    Input.Status = User.Status;
                    Input.Gender = User.Gender;

                    // Set cho đổi mật khẩu
                    PasswordInput.AccountId = User.AccountId;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(Input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7086/api/UserInfomation/update", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật thông tin thành công!";
            }
            else
            {
                TempData["Error"] = "Cập nhật thông tin thất bại.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            if (PasswordInput.NewPassword != PasswordInput.ConfirmPassword)
            {
                TempData["Error"] = "Mật khẩu xác nhận không khớp.";
                return RedirectToPage();
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 🔴 Lấy lại thông tin người dùng để có AccountId
            var infoResponse = await client.GetAsync("https://localhost:7086/api/UserInfomation/infor");
            if (!infoResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Không xác định tài khoản người dùng.";
                return RedirectToPage();
            }

            var infoJson = await infoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<UserDTO>(infoJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (userInfo == null || userInfo.AccountId == 0)
            {
                TempData["Error"] = "Không xác định tài khoản người dùng.";
                return RedirectToPage();
            }

            PasswordInput.AccountId = userInfo.AccountId;

            // 🔁 Gửi yêu cầu đổi mật khẩu
            var json = JsonSerializer.Serialize(PasswordInput);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("https://localhost:7086/api/UserInfomation/change-password", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Đổi mật khẩu thành công!";
            }
            else
            {
                TempData["Error"] = "Đổi mật khẩu thất bại.";
            }

            return RedirectToPage();
        }


        // ================================
        // ========== Models ==============
        // ================================

        public class UserDTO
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public string Gender { get; set; }
            public DateTime? DOB { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string? DoctorPath { get; set; }

            public int AccountId { get; set; }
            public string Username { get; set; }
            public int RoleId { get; set; }
            public bool Status { get; set; }

            public string? RoleName => RoleId switch
            {
                1 => "Admin",
                2 => "Doctor",
                3 => "Patient",
                _ => "Không xác định"
            };
        }

        public class UserAccountViewModel
        {
            public int AccountId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateTime? DOB { get; set; }
            public string Gender { get; set; }

            public string Username { get; set; }
            public int RoleId { get; set; }
            public bool Status { get; set; }
        }

        public class ChangePasswordViewModel
        {
            public int AccountId { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
        }
    }
}
