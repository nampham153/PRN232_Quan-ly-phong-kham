using System;
using System.Linq;
using System.Security.Claims;
using BusinessAccessLayer.IService;
using Microsoft.AspNetCore.Http;

namespace BusinessAccessLayer.IService
{
    public interface ITokenUserService
    {
        int? GetCurrentUserId(HttpContext httpContext);
        bool CanEditOrDeleteTestResult(int userId, int testResultId);
    }
}

namespace BusinessAccessLayer.Service
{
    public class TokenUserService : ITokenUserService
    {
        private readonly ITestResultService _testResultService;

        public TokenUserService(ITestResultService testResultService)
        {
            _testResultService = testResultService ?? throw new ArgumentNullException(nameof(testResultService));
        }

        public int? GetCurrentUserId(HttpContext httpContext)
        {
            try
            {
                if (httpContext?.User?.Identity?.IsAuthenticated != true)
                {
                    Console.WriteLine("GetCurrentUserId: User is not authenticated");
                    return null;
                }

                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    Console.WriteLine("GetCurrentUserId: No NameIdentifier claim found in token");
                    return null;
                }

                if (!int.TryParse(userIdClaim.Value, out int userId))
                {
                    Console.WriteLine($"GetCurrentUserId: Invalid user ID format in token: {userIdClaim.Value}");
                    return null;
                }

                Console.WriteLine($"GetCurrentUserId: Retrieved user ID: {userId}");
                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCurrentUserId: Error - {ex.Message}\nStack Trace: {ex.StackTrace}");
                return null;
            }
        }

        public bool CanEditOrDeleteTestResult(int userId, int testResultId)
        {
            try
            {
                var testResult = _testResultService.GetTestResultById(testResultId);
                if (testResult == null)
                {
                    Console.WriteLine($"CanEditOrDeleteTestResult: Test result with ID {testResultId} not found");
                    return false;
                }

                bool canEdit = testResult.UserId == userId;
                Console.WriteLine($"CanEditOrDeleteTestResult: User {userId} can edit/delete test result {testResultId}: {canEdit}");
                return canEdit;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CanEditOrDeleteTestResult: Error for testResultId {testResultId} - {ex.Message}\nStack Trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}