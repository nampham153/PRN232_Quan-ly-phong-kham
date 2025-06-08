using DataAccessLayer.models;
using DataAccessLayer.ViewModels;

namespace BusinessAccessLayer.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorVM ToViewModel(User user)
        {
            if (user == null) return null;
            return new DoctorVM
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Gender = user.Gender,
                DOB = user.DOB,
                Phone = user.Phone,
                Email = user.Email,
                AccountId = user.AccountId
            };
        }

        public static User ToEntity(DoctorVM doctorVM)
        {
            if (doctorVM == null) return null;
            return new User
            {
                UserId = doctorVM.UserId,
                FullName = doctorVM.FullName,
                Gender = doctorVM.Gender,
                DOB = doctorVM.DOB,
                Phone = doctorVM.Phone,
                Email = doctorVM.Email,
                AccountId = doctorVM.AccountId
            };
        }
    }
}
