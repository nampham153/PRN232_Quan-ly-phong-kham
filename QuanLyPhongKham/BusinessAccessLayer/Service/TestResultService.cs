using BusinessAccessLayer.IService;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Service
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultRepository _testResultRepository;
        private readonly ITestResultHistoryService _historyService;

        public TestResultService(ITestResultRepository testResultRepository, ITestResultHistoryService historyService)
        {
            _testResultRepository = testResultRepository;
            _historyService = historyService;
        }

        public List<TestResult> GetAllTestResults()
        {
            var results = _testResultRepository.GetAllTestResults();
            // Không log lịch sử cho lấy danh sách chung
            return results;
        }

        public List<TestResultVM> GetAllTestResultVMs()
        {
            return _testResultRepository.GetAllTestResultVMs();
        }

        public TestResult GetTestResultById(int id)
        {
            if (id <= 0) return null;
            var result = _testResultRepository.GetTestResultById(id);
            if (result != null)
            {
                // Ghi log lịch sử xem
                _historyService.AddHistory(new DataAccessLayer.ViewModels.TestResultHistoryVM
                {
                    UserId = result.UserId,
                    TestResultId = result.ResultId,
                    Action = "View",
                    ActionTime = DateTime.Now,
                    Note = "Xem chi tiết kết quả xét nghiệm"
                });
            }
            return result;
        }

        public List<TestResult> GetTestResultsByRecordId(int recordId)
        {
            if (recordId <= 0) return new List<TestResult>();
            return _testResultRepository.GetTestResultsByRecordId(recordId);
        }

        public List<TestResult> GetTestResultsByUserId(int userId)
        {
            if (userId <= 0) return new List<TestResult>();
            return _testResultRepository.GetTestResultsByUserId(userId);
        }

        public bool CreateTestResult(TestResultVM testResultVM)
        {
            if (!ValidateTestResult(testResultVM, out List<string> errors))
                return false;

            var success = _testResultRepository.AddTestResult(testResultVM);
            if (success)
            {
                // Lấy lại TestResult vừa tạo (giả sử UserId, TestId, RecordId, TestDate là duy nhất)
                var created = _testResultRepository.GetAllTestResults()
                    .FirstOrDefault(tr => tr.UserId == testResultVM.UserId && tr.TestId == testResultVM.TestId && tr.RecordId == testResultVM.RecordId && tr.TestDate == testResultVM.TestDate);
                if (created != null)
                {
                    _historyService.AddHistory(new DataAccessLayer.ViewModels.TestResultHistoryVM
                    {
                        UserId = created.UserId,
                        TestResultId = created.ResultId,
                        Action = "Create",
                        ActionTime = DateTime.Now,
                        Note = "Tạo mới kết quả xét nghiệm"
                    });
                }
            }
            return success;
        }

        public bool UpdateTestResult(int id, TestResultVM testResultVM)
        {
            if (id <= 0) return false;

            if (!ValidateTestResult(testResultVM, out List<string> errors))
                return false;

            if (!_testResultRepository.TestResultExists(id))
                return false;

            // Lấy giá trị cũ trước khi update (clone lại để tránh bị thay đổi bởi EF)
            var oldResultEntity = _testResultRepository.GetTestResultById(id);
            TestResult oldResult = null;
            if (oldResultEntity != null)
            {
                oldResult = new TestResult
                {
                    ResultId = oldResultEntity.ResultId,
                    RecordId = oldResultEntity.RecordId,
                    TestId = oldResultEntity.TestId,
                    UserId = oldResultEntity.UserId,
                    ResultDetail = oldResultEntity.ResultDetail,
                    TestDate = oldResultEntity.TestDate
                };
            }

            var success = _testResultRepository.UpdateTestResult(id, testResultVM);
            if (success)
            {
                // So sánh và tạo note chi tiết
                var changes = new List<string>();
                if (oldResult != null)
                {
                    if (oldResult.RecordId != testResultVM.RecordId)
                        changes.Add($"recordId: {oldResult.RecordId} => {testResultVM.RecordId}");
                    if (oldResult.TestId != testResultVM.TestId)
                        changes.Add($"testId: {oldResult.TestId} => {testResultVM.TestId}");
                    if (oldResult.UserId != testResultVM.UserId)
                        changes.Add($"userId: {oldResult.UserId} => {testResultVM.UserId}");
                    // Log rõ cả khi null/rỗng
                    var oldDetail = oldResult.ResultDetail ?? "<null>";
                    var newDetail = testResultVM.ResultDetail ?? "<null>";
                    if (oldDetail != newDetail)
                        changes.Add($"resultDetail: '{oldDetail}' => '{newDetail}'");
                    if (oldResult.TestDate != testResultVM.TestDate)
                        changes.Add($"testDate: {oldResult.TestDate:dd/MM/yyyy HH:mm} => {testResultVM.TestDate:dd/MM/yyyy HH:mm}");
                }
                var note = changes.Count > 0
                    ? $"Cập nhật kết quả xét nghiệm: {string.Join(", ", changes)}"
                    : "Cập nhật kết quả xét nghiệm (không thay đổi dữ liệu)";
                _historyService.AddHistory(new DataAccessLayer.ViewModels.TestResultHistoryVM
                {
                    UserId = testResultVM.UserId,
                    TestResultId = id,
                    Action = "Update",
                    ActionTime = DateTime.Now,
                    Note = note
                });
            }
            return success;
        }

        public bool DeleteTestResult(int id)
        {
            if (id <= 0) return false;

            if (!_testResultRepository.TestResultExists(id))
                return false;

            // Lấy thông tin trước khi xóa để log
            var testResult = _testResultRepository.GetTestResultById(id);
            var success = _testResultRepository.DeleteTestResult(id);
            if (success && testResult != null)
            {
                _historyService.AddHistory(new DataAccessLayer.ViewModels.TestResultHistoryVM
                {
                    UserId = testResult.UserId,
                    TestResultId = id,
                    Action = "Delete",
                    ActionTime = DateTime.Now,
                    Note = "Xóa kết quả xét nghiệm"
                });
            }
            return success;
        }

        public bool TestResultExists(int id)
        {
            return _testResultRepository.TestResultExists(id);
        }

        public TestResultVM GetTestResultForEdit(int id)
        {
            if (id <= 0) return null;
            return _testResultRepository.GetTestResultVMById(id);
        }

        public TestResultVM GetNewTestResultVM()
        {
            return _testResultRepository.CreateNewTestResultVM();
        }

        public bool ValidateTestResult(TestResultVM testResultVM, out List<string> errors)
        {
            errors = new List<string>();

            if (testResultVM == null)
            {
                errors.Add("Test result data is required");
                return false;
            }

            if (testResultVM.RecordId <= 0)
                errors.Add("Valid Medical Record is required");

            if (testResultVM.TestId <= 0)
                errors.Add("Valid Test is required");

            if (testResultVM.UserId <= 0)
                errors.Add("Valid User is required");

            if (string.IsNullOrWhiteSpace(testResultVM.ResultDetail))
                errors.Add("Result Detail is required");
            else if (testResultVM.ResultDetail.Length > 1000)
                errors.Add("Result Detail cannot exceed 1000 characters");

            if (testResultVM.TestDate == default(DateTime))
                errors.Add("Valid Test Date is required");
            else if (testResultVM.TestDate > DateTime.Now)
                errors.Add("Test Date cannot be in the future");

            return errors.Count == 0;
        }
    }
}