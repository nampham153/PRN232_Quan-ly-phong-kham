using DataAccessLayer.DAO;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;

namespace DataAccessLayer.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDAO _doctorDao;

        public DoctorRepository(DoctorDAO doctorDao)
        {
            _doctorDao = doctorDao;
        }

        public List<User> GetAllDoctors(string searchTerm = "")
        {
            var query = _doctorDao.GetAllDoctors();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FullName.Contains(searchTerm));
            }

            return query.ToList();
        }

        public User? GetDoctorByAccountId(int accountId)
        {
            return _doctorDao.GetByAccountId(accountId);
        }

        public bool CanCreateDoctor(int accountId, out string errorMessage)
        {
            errorMessage = "";

            if (_doctorDao.ExistsUserByAccountId(accountId))
            {
                errorMessage = "Tài khoản này đã được đăng ký";
                return false;
            }

            var account = _doctorDao.GetAccount(accountId);
            if (account == null || account.RoleId != 2)
            {
                errorMessage = "Vui lòng nhập lại AccountId";
                return false;
            }

            if (!account.Status)
            {
                errorMessage = "Tài khoản này đã bị vô hiệu hóa";
                return false;
            }

            return true;
        }


        public void CreateDoctor(User doctor)
        {
            _doctorDao.Add(doctor);
        }

        public void UpdateDoctor(User doctor)
        {
            _doctorDao.Update(doctor);
        }

        public bool IsEmailExists(string email)
        {
            return _doctorDao.GetAllDoctors()
                .Any(d => d.Email == email && d.Account != null && d.Account.Status == true);
        }

        public bool IsPhoneExists(string phone)
        {
            return _doctorDao.GetAllDoctors()
                .Any(d => d.Phone == phone && d.Account != null && d.Account.Status == true);
        }

    }
}
