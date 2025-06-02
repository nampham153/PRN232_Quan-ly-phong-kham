using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ClinicDbContext _context;

        public DoctorRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllDoctors(string searchTerm = "")
        {
            var query = _context.Users
                .Include(u => u.Account)
                .Where(u => u.Account.RoleId == 2);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FullName.Contains(searchTerm));
            }

            return query.ToList();
        }


        public User GetDoctorByAccountId(int accountId)
        {
            return _context.Users
                .Include(u => u.Account)
                .FirstOrDefault(u => u.AccountId == accountId && u.Account.RoleId== 2);
        }

        public bool CanCreateDoctor(int accountId, out string errorMessage)
        {
            errorMessage = "";

            bool existsUser = _context.Users.Any(u => u.AccountId == accountId);
            if (existsUser)
            {
                errorMessage = "Tài khoản này đã được đăng ký";
                return false;
            }

            var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null || account.RoleId != 2)
            {
                errorMessage = "Vui lòng nhập lại AccountId";
                return false;
            }

            return true;
        }

        public void CreateDoctor(User doctor)
        {
            _context.Users.Add(doctor);
            _context.SaveChanges();
        }

        public void UpdateDoctor(User doctor)
        {
            _context.Users.Update(doctor);
            _context.SaveChanges();
        }

        public void DeleteDoctor(User doctor)
        {
            _context.Users.Remove(doctor);
            _context.SaveChanges();
        }
    }


}
