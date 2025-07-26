using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels.Authen;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Service.Authen
{
    public class ManagerUserDAO
    {
        private readonly ClinicDbContext _context;

        public ManagerUserDAO(ClinicDbContext context)
        {
            _context = context;
        }

        // 1 + 3: Danh sách tài khoản có search + phân trang (6 account / trang)
        public List<AccountDTO> GetAccounts(string searchKeyword = "", int pages = 1, int? roleId = null, bool? status = null)
        {
            var query = _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.User)
                .Include(a => a.Patient).
                Where(a => a.IsCheck)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                query = query.Where(a =>
                    a.Username.Contains(searchKeyword) ||
                    (a.User != null && (
                        a.User.FullName.Contains(searchKeyword) ||
                        a.User.Email.Contains(searchKeyword)
                    )));
            }

            if (roleId.HasValue)
            {
                query = query.Where(a => a.RoleId == roleId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            return query
                .OrderBy(a => a.AccountId)
                .Skip((pages - 1) * 6)
                .Take(6)
                .Select(a => new AccountDTO
                {
                    AccountId = a.AccountId,
                    Username = a.Username,
                    RoleName = a.Role != null ? a.Role.RoleName : null,
                    Email = a.User != null ? a.User.Email : null,
                    Status = a.Status
                })
                .ToList();
        }

        // Lấy danh sách Role (async)
        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<int> CountAccountsAsync(string searchKeyword, int? roleId, bool? status)
        {
            var query = _context.Accounts
                .Include(a => a.User)
                .Where(a => a.IsCheck)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(a =>
                    a.Username.Contains(searchKeyword) ||
                    (a.User != null && (
                        a.User.FullName.Contains(searchKeyword) ||
                        a.User.Email.Contains(searchKeyword))));
            }

            if (roleId.HasValue)
            {
                query = query.Where(a => a.RoleId == roleId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            return await query.CountAsync();
        }

        // 4: Tạo tài khoản mới (Status mặc định = false)
        public bool CreateAccount(UserAccountViewModel accountViewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountViewModel.Username) || string.IsNullOrWhiteSpace(accountViewModel.Password))
                    throw new ArgumentException("Username và Password không được để trống.");

                // ✅ Kiểm tra trùng Username
                var existingAccount = _context.Accounts.FirstOrDefault(a => a.Username == accountViewModel.Username);
                if (existingAccount != null)
                    throw new ArgumentException("Tên đăng nhập đã tồn tại.");

                var role = _context.Roles.Find(accountViewModel.RoleId);
                if (role == null)
                    throw new ArgumentException("Role không hợp lệ.");

                // ✅ Validate password: tối thiểu 8 ký tự, bắt đầu chữ hoa, kết thúc ký tự đặc biệt
                var password = accountViewModel.Password;

                if (password.Length < 8)
                    throw new ArgumentException("Mật khẩu phải có ít nhất 8 ký tự.");

                if (!char.IsUpper(password[0]))
                    throw new ArgumentException("Mật khẩu phải bắt đầu bằng chữ hoa.");

                if (!IsSpecialCharacter(password[^1]))
                    throw new ArgumentException("Mật khẩu phải kết thúc bằng ký tự đặc biệt.");

                // TODO: Hash mật khẩu nếu cần
                string hashedPassword = password; // Nên hash mật khẩu thật nhé

                var account = new Account
                {
                    Username = accountViewModel.Username,
                    PasswordHash = hashedPassword,
                    Status = accountViewModel.Status, // lấy giá trị truyền vào
                    RoleId = accountViewModel.RoleId,
                    CreatedAt = DateTime.Now // Gán đúng thời gian

                };

                _context.Accounts.Add(account);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine("CreateAccount error: " + ex.Message);
                return false;
            }
        }

        // Hàm kiểm tra ký tự đặc biệt (bạn có thể sửa danh sách ký tự đặc biệt phù hợp)
        private bool IsSpecialCharacter(char c)
        {
            const string specialChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?";
            return specialChars.Contains(c);
        }



        // 5: Xem chi tiết tài khoản theo ID
        public AccountDTO GetAccountById(int id)
        {
            var account = _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.User).Where(a => a.IsCheck)
                .FirstOrDefault(a => a.AccountId == id);

            if (account == null) return null;

            return new AccountDTO
            {
                AccountId = account.AccountId,
                Username = account.Username,
                RoleName = account.Role?.RoleName,
                RoleId = account.RoleId, // ✅ thêm
                Email = account.User?.Email,
                FullName = account.User?.FullName ?? "", // ✅ thêm
                Status = account.Status
            };
        }



        // 6: Xóa tài khoản nếu Status = false
        public bool DeleteAccount(int accountId)
        {
            try
            {
                var account = _context.Accounts
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.MedicalRecords)
                    .Include(a => a.User)
                    .Include(a => a.RefreshToken)
                    .FirstOrDefault(a => a.AccountId == accountId);

                if (account == null || account.Status)
                    return false;

                if (account.Patient?.MedicalRecords != null && account.Patient.MedicalRecords.Any())
                    return false;

                if (account.User != null)
                    return false;

                if (account.RefreshToken != null)
                    return false;

                // Đánh dấu xóa mềm
                account.IsCheck = false;
                account.UpdatedAt = DateTime.UtcNow;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        // 7: Cập nhật tài khoản theo ID
        public bool UpdateAccount(UserAccountViewModel updatedAccount)
        {
            try
            {
                var existingAccount = _context.Accounts.Find(updatedAccount.AccountId);
                if (existingAccount != null)
                {
                    var role = _context.Roles.FirstOrDefault(r => r.RoleId == updatedAccount.RoleId);
                    if (role == null)
                        return false; // hoặc throw/lỗi nếu Role không tồn tại

                    existingAccount.Username = updatedAccount.Username;
                    existingAccount.RoleId = role.RoleId;

                    // Nếu muốn cập nhật FullName:
                    if (existingAccount.User != null)
                    {
                        existingAccount.User.FullName = updatedAccount.FullName;
                    }

                    // Nếu muốn cập nhật trạng thái:
                    existingAccount.Status = updatedAccount.Status;

                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Lấy thông tin User theo AccountId
        public User? GetUserByAccountId(int accountId)
        {
            return _context.Users
                           .Include(u => u.Account) // nếu cần thêm thông tin tài khoản
                           .FirstOrDefault(u => u.AccountId == accountId);
        }

        // Nếu muốn trả DTO thay vì entity (tùy chọn)
        public UserDTO? GetUserDtoByAccountId(int accountId)
        {
            return _context.Users
                           .Where(u => u.AccountId == accountId)
                           .Select(u => new UserDTO
                           {
                               UserId = u.UserId,
                               FullName = u.FullName,
                               Gender = u.Gender,
                               DOB = u.DOB,
                               Phone = u.Phone,
                               Email = u.Email,
                               AccountId = u.AccountId,
                               DoctorPath = u.DoctorPath
                           })
                           .FirstOrDefault();
        }

        //Cập nhật thông tin cá nhân 
        public bool UpdateUserInformation(ChangeInformationViewModel updatedInfo)
        {
            try
            {
                var existingAccount = _context.Accounts
                    .Include(a => a.User)
                    .FirstOrDefault(a => a.AccountId == updatedInfo.AccountId);

                if (existingAccount == null || existingAccount.User == null)
                    return false;

                // Cập nhật Account
                existingAccount.Username = updatedInfo.Username;
                existingAccount.RoleId = updatedInfo.RoleId;
                existingAccount.Status = updatedInfo.Status;

                // Cập nhật User
                existingAccount.User.FullName = updatedInfo.FullName;
                existingAccount.User.Email = updatedInfo.Email;
                existingAccount.User.Phone = updatedInfo.Phone;
                existingAccount.User.DOB = updatedInfo.DOB;
                existingAccount.User.Gender = updatedInfo.Gender;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //thay dổi mật khẩu đăng nhập
        public bool ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var account = _context.Accounts.FirstOrDefault(a => a.AccountId == model.AccountId);
                if (account == null)
                    return false;

                if (string.IsNullOrEmpty(model.OldPassword) ||
                    string.IsNullOrEmpty(model.NewPassword) ||
                    string.IsNullOrEmpty(model.ConfirmPassword))
                    return false;

                if (model.NewPassword != model.ConfirmPassword)
                    return false;

                // So sánh mật khẩu hiện tại (tạm thời dùng plain text)
                if (model.OldPassword != account.PasswordHash)
                    return false;

                // Gán mật khẩu mới
                account.PasswordHash = model.NewPassword;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
