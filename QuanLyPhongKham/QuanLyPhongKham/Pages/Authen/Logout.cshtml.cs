using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuanLyPhongKham.Pages.Authen
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xóa session
            HttpContext.Session.Clear();

            // Chuyển về trang đăng nhập
            return RedirectToPage("/Authen/Login");
        }
    }
}
