using BusinessAccessLayer.IService.Authen;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels.Authen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusinessAccessLayer.Service.Authen
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserInfomationController : ControllerBase
    {
        private readonly IManagerUserService _userService;

        public UserInfomationController(IManagerUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("infor")]
        public ActionResult<UserDTO> GetUserDtoFromEntity()
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
                return Unauthorized("Không tìm thấy claim AccountId trong token");

            int accountId = int.Parse(accountIdClaim.Value);

            var user = _userService.GetUserEntity(accountId); // Trả về User entity đầy đủ
            if (user == null)
                return NotFound("User không tồn tại");

            // ✅ Map thủ công từ User → UserDTO
            var userDto = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Gender = user.Gender.ToString(),
                DOB = user.DOB,
                Phone = user.Phone,
                Email = user.Email,
                DoctorPath = user.DoctorPath,

                AccountId = user.Account?.AccountId ?? 0,
                Username = user.Account?.Username ?? "",
                RoleId = user.Account?.RoleId ?? 0,
                Status = user.Account?.Status ?? false
            };

            return Ok(userDto);
        }


        // GET: api/user/dto/5
        [HttpGet("information")]
        public ActionResult<UserDTO> GetUserDto()
        {
            var accountIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
                return Unauthorized("Không tìm thấy claim AccountId trong token");

            int accountId = int.Parse(accountIdClaim.Value);

            var userDto = _userService.GetUserDto(accountId);
            if (userDto == null)
                return NotFound("User không tồn tại");

            return Ok(userDto);
        }

        [HttpPut("update")]
        public IActionResult UpdateInfor([FromBody] ChangeInformationViewModel model)
        {
            var success = _userService.UpdateAccountInfor(model);
            if (!success)
                return BadRequest("Cập nhật thông tin thất bại.");

            return Ok("Cập nhật thông tin thành công.");
        }

        [HttpPut("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var success = _userService.ChangepassInfor(model);
            if (!success)
                return BadRequest("Đổi mật khẩu thất bại.");

            return Ok("Đổi mật khẩu thành công.");
        }

    }
}
