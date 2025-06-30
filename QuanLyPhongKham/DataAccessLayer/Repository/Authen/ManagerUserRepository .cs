using BusinessAccessLayer.Service.Authen;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository.Authen;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels.Authen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.Authen
{
    public class ManagerUserRepository : IManagerUserRepository
    {
        private readonly ManagerUserDAO _accountDAO;

        public ManagerUserRepository(ManagerUserDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }

        public List<AccountDTO> GetAccounts(string searchKeyword, int pages, int? roleId, bool? status)
        {
            return _accountDAO.GetAccounts(searchKeyword, pages, roleId, status);
        }
        public async Task<int> CountAccountsAsync(string searchKeyword, int? roleId, bool? status)
        {
            return await _accountDAO.CountAccountsAsync(searchKeyword, roleId, status);
        }


        public bool CreateAccount(UserAccountViewModel account)
        {
            return _accountDAO.CreateAccount(account);
        }

        public AccountDTO GetAccountById(int accountId)
        {
            return _accountDAO.GetAccountById(accountId);
        }

        public bool DeleteAccount(int accountId)
        {
            return _accountDAO.DeleteAccount(accountId);
        }

        public bool UpdateAccount(UserAccountViewModel updatedAccount)
        {
            return _accountDAO.UpdateAccount(updatedAccount);
        }
        public async Task<List<Role>> GetRolesAsync()
        {
            return await _accountDAO.GetRolesAsync();
        }

        // Trả về entity User (bao gồm cả Account nếu cần)
        public User? GetUserByAccountId(int accountId)
        {
            return _accountDAO.GetUserByAccountId(accountId);
        }

        // Trả về DTO để dùng cho view hoặc API
        public UserDTO? GetUserDtoByAccountId(int accountId)
        {
            return _accountDAO.GetUserDtoByAccountId(accountId);
        }
        public bool UpdateAccountInfor(ChangeInformationViewModel model)
        {
            return _accountDAO.UpdateUserInformation(model);
        }

        public bool ChangepassInfor(ChangePasswordViewModel model)
        {
            return _accountDAO.ChangePassword(model);
        }
    }

}
