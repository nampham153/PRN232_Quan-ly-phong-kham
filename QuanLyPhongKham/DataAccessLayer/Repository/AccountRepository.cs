using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DataAccessLayer.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ClinicDbContext _context;

        public AccountRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public Account GetAccountById(int accountid)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == accountid);
        }

    }
}
