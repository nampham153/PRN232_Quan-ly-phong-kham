using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels.Authen
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }

        public string Email { get; set; }
        public bool Status { get; set; }
    }

}
