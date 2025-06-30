// Login.cshtml.cs
using DataAccessLayer.ViewModels.Authen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.OData.UriParser;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Linq;

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

            // Giải mã token để lấy role
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            foreach (var claim in jwtToken.Claims)
            {
                Console.WriteLine($"Claim type: {claim.Type}, value: {claim.Value}");
                ErrorMessage += $"{claim.Type}: {claim.Value} | ";
            }

            var role = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Role ||
                c.Type.EndsWith("/claims/role") ||
                c.Type.Contains("role"))?.Value;

            HttpContext.Session.SetString("UserRole", role);

            if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/Authen/Index");
            else if (role.Equals("doctor", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/TestResultPage/Index");
            else if (role.Equals("patient", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("/Patient/Index");


            ErrorMessage = "Không xác định được vai trò người dùng.";
            return Page();
        }



    }
}
