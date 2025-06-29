using DataAccessLayer.models;
using DataAccessLayer.ViewModels.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepository.Authen
{
    public interface IManagerUserRepository
    {
        List<AccountDTO> GetAccounts(string searchKeyword, int pages, int? roleId, bool? status);
        Task<int> CountAccountsAsync(string searchKeyword, int? roleId, bool? status);
        bool CreateAccount(UserAccountViewModel account);
        AccountDTO GetAccountById(int accountId);
        bool DeleteAccount(int accountId);
        bool UpdateAccount(UserAccountViewModel updatedAccount);
        Task<List<Role>> GetRolesAsync();

    }

}
