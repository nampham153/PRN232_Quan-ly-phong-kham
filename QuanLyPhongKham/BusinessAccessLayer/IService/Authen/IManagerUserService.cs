using DataAccessLayer.models;
using DataAccessLayer.ViewModels.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.IService.Authen
{
    public interface IManagerUserService
    {
        List<AccountDTO> GetAccounts(string searchKeyword, int pages, int? roleId, bool? status);
        Task<int> CountAccountsAsync(string searchKeyword, int? roleId, bool? status);

        Task<List<Role>> GetRolesAsync();

        bool CreateAccount(UserAccountViewModel account);
        AccountDTO GetAccountById(int accountId);
        bool DeleteAccount(int accountId);
        bool UpdateAccount(UserAccountViewModel updatedAccount);

        User? GetUserEntity(int accountId);
        UserDTO? GetUserDto(int accountId);

        bool UpdateAccountInfor(ChangeInformationViewModel model);
        bool ChangepassInfor(ChangePasswordViewModel model);


    }

}
