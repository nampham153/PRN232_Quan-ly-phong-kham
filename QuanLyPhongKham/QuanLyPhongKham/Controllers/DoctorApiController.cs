using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using DataAccessLayer.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.IRepository;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorApiController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAccountRepository _accountRepository;

        // ✅ Constructor hợp lệ duy nhất
        public DoctorApiController(IDoctorService doctorService, IAccountRepository accountRepository)
        {
            _doctorService = doctorService;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = _doctorService.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet("{accountId}")]
        public IActionResult GetDoctorByAccountId(int accountId)
        {
            var doctor = _doctorService.GetDoctorByAccountId(accountId);
            if (doctor == null)
                return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult CreateDoctor([FromBody] DoctorVM doctorVM)
        {
            // Kiểm tra nếu tài khoản đã được đăng ký
            var existingDoctor = _doctorService.GetDoctorByAccountId(doctorVM.AccountId);
            if (existingDoctor != null)
            {
                return BadRequest("Tài khoản này đã được đăng ký.");
            }

            // Lấy thông tin tài khoản từ AccountRepository
            var account = _accountRepository.GetAccountById(doctorVM.AccountId);
            if (account == null || account.RoleId != 2)
            {
                return BadRequest("Vui lòng nhập lại AccountId.");
            }

            // Nếu hợp lệ thì tạo bác sĩ
            var doctorEntity = new User
            {
                FullName = doctorVM.FullName,
                Gender = doctorVM.Gender,
                DOB = doctorVM.DOB,
                Phone = doctorVM.Phone,
                Email = doctorVM.Email,
                AccountId = doctorVM.AccountId
            };

            _doctorService.CreateDoctor(doctorEntity);
            return Ok("Tạo bác sĩ thành công.");
        }

        [HttpPut("{accountId}")]
        public IActionResult UpdateDoctor(int accountId, [FromBody] DoctorVM doctorVM)
        {
            var doctorEntity = new User
            {
                UserId = doctorVM.UserId,
                FullName = doctorVM.FullName,
                Gender = doctorVM.Gender,
                DOB = doctorVM.DOB,
                Phone = doctorVM.Phone,
                Email = doctorVM.Email,
                AccountId = doctorVM.AccountId
            };

            _doctorService.UpdateDoctor(accountId, doctorEntity);
            return Ok();
        }

        [HttpDelete("{accountId}")]
        public IActionResult DeleteDoctor(int accountId)
        {
            _doctorService.DeleteDoctor(accountId);
            return Ok();
        }
    }
}
