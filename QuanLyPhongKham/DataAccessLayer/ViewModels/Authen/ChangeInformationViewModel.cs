using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels.Authen
{
    public class ChangeInformationViewModel
    {
        // Thông tin người dùng
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }  // "Male" / "Female" hoặc "True"/"False"
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Thông tin tài khoản
        public string Username { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }  // Nếu cần cập nhật trạng thái hoạt động
    }
}
