using BusinessAccessLayer.IService;
using DataAccessLayer.ViewModels;
using DataAccessLayer.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.IRepository;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]

    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAccountRepository _accountRepository;
        private readonly IWebHostEnvironment _env;

        public DoctorController(IDoctorService doctorService, IAccountRepository accountRepository, IWebHostEnvironment env)
        {
            _doctorService = doctorService;
            _accountRepository = accountRepository;
            _env = env;
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            try
            {
                var doctors = _doctorService.GetAllDoctors();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi GetAllDoctors: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }

        [HttpGet("{accountId}")]
        public IActionResult GetDoctorByAccountId(int accountId)
        {
            try
            {
                var doctor = _doctorService.GetDoctorByAccountId(accountId);
                if (doctor == null)
                    return NotFound();
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi GetDoctorByAccountId: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
        [HttpPost]
        public IActionResult CreateDoctor([FromBody] DoctorVM doctorVM)
        {

            var existingDoctor = _doctorService.GetDoctorByAccountId(doctorVM.AccountId);
            if (existingDoctor != null)
            {
                return BadRequest("Tài khoản này đã được đăng ký.");
            }

            var account = _accountRepository.GetAccountById(doctorVM.AccountId);
            if (account == null || account.RoleId != 2)
            {
                return BadRequest("Vui lòng nhập lại AccountId.");
            }

            var doctorEntity = new User
            {
                FullName = doctorVM.FullName,
                Gender = doctorVM.Gender,
                DOB = doctorVM.DOB,
                Phone = doctorVM.Phone,
                Email = doctorVM.Email,
                AccountId = doctorVM.AccountId,
                DoctorPath = doctorVM.DoctorPath
            };

            _doctorService.CreateDoctor(doctorEntity);
            return Ok("Tạo bác sĩ thành công.");
        }

        [HttpPut("{accountId}")]
        public IActionResult UpdateDoctor(int accountId, [FromBody] DoctorVM doctorVM)
        {
            try
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
                return Ok("Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi UpdateDoctor: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
        [HttpGet("accounts/available")]
        public IActionResult GetAvailableDoctorAccounts()
        {
            try
            {
                var accounts = _accountRepository.GetAvailableAccountsForDoctor();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi GetAvailableDoctorAccounts: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }


        [HttpDelete("{accountId}")]
        public IActionResult DeleteDoctor(int accountId)
        {
            try
            {
                _doctorService.DeleteDoctor(accountId);
                return Ok("Xóa thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DeleteDoctor: " + ex.Message);
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
    }
}
