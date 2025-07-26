using BusinessAccessLayer.IService;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyPhongKham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAccountRepository _accountRepository;

        public DoctorController(IDoctorService doctorService, IAccountRepository accountRepository)
        {
            _doctorService = doctorService;
            _accountRepository = accountRepository;
        }

        // GET api/doctor?name=...&email=...&page=1&pageSize=10
        [HttpGet]
        public IActionResult GetDoctors([FromQuery] string? name = null,
                                        [FromQuery] string? email = null,
                                        [FromQuery] int page = 1,
                                        [FromQuery] int pageSize = 10)
        {
            try
            {
                var doctors = _doctorService.GetAllDoctors();

                if (!string.IsNullOrEmpty(name))
                    doctors = doctors.Where(d => !string.IsNullOrEmpty(d.FullName) && d.FullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!string.IsNullOrEmpty(email))
                    doctors = doctors.Where(d => !string.IsNullOrEmpty(d.Email) && d.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();

                var total = doctors.Count;

                doctors = doctors
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    Data = doctors,
                    TotalRecords = total,
                    Page = page,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi truy xuất danh sách bác sĩ", error = ex.Message });
            }
        }

        [HttpGet("{accountId}")]
        public IActionResult GetDoctorByAccountId(int accountId)
        {
            try
            {
                var doctor = _doctorService.GetDoctorByAccountId(accountId);
                if (doctor == null) return NotFound();
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi truy xuất bác sĩ", error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateDoctor([FromBody] DoctorVM doctorVM)
        {
            var existingDoctor = _doctorService.GetDoctorByAccountId(doctorVM.AccountId);
            if (existingDoctor != null)
                return BadRequest("Tài khoản này đã được đăng ký.");

            var account = _accountRepository.GetAccountById(doctorVM.AccountId);
            if (account == null || account.RoleId != 2)
                return BadRequest("Vui lòng nhập lại AccountId hợp lệ.");

            if (_doctorService.IsEmailExists(doctorVM.Email))
                return BadRequest("Email đã được sử dụng bởi bác sĩ khác.");

            if (_doctorService.IsPhoneExists(doctorVM.Phone))
                return BadRequest("Số điện thoại đã được sử dụng bởi bác sĩ khác.");
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
                var existingDoctor = _doctorService.GetDoctorByAccountId(accountId);
                if (!string.Equals(existingDoctor.Email, doctorVM.Email, StringComparison.OrdinalIgnoreCase)
                    && _doctorService.IsEmailExists(doctorVM.Email))
                {
                    return BadRequest("Email đã được sử dụng bởi bác sĩ khác.");
                }
                if (!string.Equals(existingDoctor.Phone, doctorVM.Phone, StringComparison.OrdinalIgnoreCase)
                    && _doctorService.IsPhoneExists(doctorVM.Phone))
                {
                    return BadRequest("Số điện thoại đã được sử dụng bởi bác sĩ khác.");
                }
                var doctorEntity = new User
                {
                    UserId = doctorVM.UserId,
                    FullName = doctorVM.FullName,
                    Gender = doctorVM.Gender,
                    DOB = doctorVM.DOB,
                    Phone = doctorVM.Phone,
                    Email = doctorVM.Email,
                    AccountId = doctorVM.AccountId,
                    DoctorPath = doctorVM.DoctorPath,
                };

                _doctorService.UpdateDoctor(accountId, doctorEntity);
                return Ok("Cập nhật bác sĩ thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi cập nhật bác sĩ", error = ex.Message });
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
                return StatusCode(500, new { message = "Lỗi lấy tài khoản khả dụng", error = ex.Message });
            }
        }

        [HttpGet("names")]
        public IActionResult GetDistinctDoctorNames()
        {
            try
            {
                var names = _doctorService.GetAllDoctors()
                    .Where(d => !string.IsNullOrEmpty(d.FullName))
                    .Select(d => d.FullName!)
                    .Distinct()
                    .OrderBy(n => n)
                    .ToList();

                return Ok(names);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi lấy tên bác sĩ", error = ex.Message });
            }
        }

        [HttpGet("emails")]
        public IActionResult GetDistinctDoctorEmails()
        {
            try
            {
                var emails = _doctorService.GetAllDoctors()
                    .Where(d => !string.IsNullOrEmpty(d.Email))
                    .Select(d => d.Email!)
                    .Distinct()
                    .OrderBy(e => e)
                    .ToList();

                return Ok(emails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi lấy email bác sĩ", error = ex.Message });
            }
        }
    }
}
