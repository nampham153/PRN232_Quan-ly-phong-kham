using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace QuanLyPhongKham.Pages.Authen
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        public AccountDTO Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Authen/Login");

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7086/api/account/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            Account = await response.Content.ReadFromJsonAsync<AccountDTO>();
            if (Account == null)
                return NotFound("Không tìm thấy tài khoản.");

            return Page();
        }

        // ✅ Khai báo DTO bên trong class PageModel (nội bộ dùng thôi)
        public class AccountDTO
        {
            public int AccountId { get; set; }
            public string Username { get; set; }
            public string RoleName { get; set; }
            public string Email { get; set; }
            public bool Status { get; set; }
        }
    }
}
