using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels.Authen
{
    public class UserDTO
    {
        // Thông tin người dùng
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int AccountId { get; set; }
        public string? DoctorPath { get; set; }

        // Thông tin tài khoản (Account)
        public string Username { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }

        // Tuỳ chọn: RoleName từ RoleId (nếu muốn show tên vai trò)
        public string? RoleName => RoleId switch
        {
            1 => "Admin",
            2 => "Doctor",
            3 => "Patient",
            _ => "Không xác định"
        };
    }
}
