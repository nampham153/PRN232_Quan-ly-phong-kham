using DataAccessLayer.dbcontext;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repository
{
    public class TestResultHistoryRepository : ITestResultHistoryRepository
    {
        private readonly ClinicDbContext _context;
        public TestResultHistoryRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public List<TestResultHistory> GetAllHistories()
        {
            return _context.TestResultHistories.ToList();
        }

        public List<TestResultHistoryVM> GetAllHistoryVMs()
        {
            return _context.TestResultHistories.Select(h => new TestResultHistoryVM
            {
                Id = h.Id,
                UserId = h.UserId,
                TestResultId = h.TestResultId,
                Action = h.Action,
                ActionTime = h.ActionTime,
                Note = h.Note
            }).ToList();
        }

        public List<TestResultHistory> GetHistoriesByUserId(int userId)
        {
            return _context.TestResultHistories.Where(h => h.UserId == userId).ToList();
        }

        public List<TestResultHistory> GetHistoriesByTestResultId(int testResultId)
        {
            return _context.TestResultHistories.Where(h => h.TestResultId == testResultId).ToList();
        }

        public bool AddHistory(TestResultHistoryVM historyVM)
        {
            var entity = new TestResultHistory
            {
                UserId = historyVM.UserId,
                TestResultId = historyVM.TestResultId,
                Action = historyVM.Action,
                ActionTime = historyVM.ActionTime,
                Note = historyVM.Note
            };
            _context.TestResultHistories.Add(entity);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteHistory(int id)
        {
            var entity = _context.TestResultHistories.Find(id);
            if (entity == null) return false;
            _context.TestResultHistories.Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public TestResultHistory GetHistoryById(int id)
        {
            return _context.TestResultHistories.FirstOrDefault(h => h.Id == id);
        }

        public bool HistoryExists(int id)
        {
            return _context.TestResultHistories.Any(h => h.Id == id);
        }
    }
}