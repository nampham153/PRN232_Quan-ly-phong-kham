// Login.cshtml.cs
using DataAccessLayer.ViewModels.Authen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace QuanLyPhongKham.Pages.Authen
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync("https://localhost:7086/api/auth/login", LoginRequest);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
                return Page();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var token = jsonDoc.RootElement.GetProperty("token").GetString();

            // Lưu token vào session
            HttpContext.Session.SetString("JWTToken", token);

            // Redirect sang trang admin
            return RedirectToPage("/Authen/Index"); // ✅ đúng cú pháp Razor Pages
        }
    }
}
