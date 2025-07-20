using BusinessAccessLayer.IService;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System.Collections.Generic;

namespace BusinessAccessLayer.Service
{
    public class TestResultHistoryService : ITestResultHistoryService
    {
        private readonly ITestResultHistoryRepository _historyRepository;
        public TestResultHistoryService(ITestResultHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }
        public List<TestResultHistory> GetAllHistories() => _historyRepository.GetAllHistories();
        public List<TestResultHistoryVM> GetAllHistoryVMs() => _historyRepository.GetAllHistoryVMs();
        public List<TestResultHistory> GetHistoriesByUserId(int userId) => _historyRepository.GetHistoriesByUserId(userId);
        public List<TestResultHistory> GetHistoriesByTestResultId(int testResultId) => _historyRepository.GetHistoriesByTestResultId(testResultId);
        public bool AddHistory(TestResultHistoryVM historyVM) => _historyRepository.AddHistory(historyVM);
        public bool DeleteHistory(int id) => _historyRepository.DeleteHistory(id);
        public TestResultHistory GetHistoryById(int id) => _historyRepository.GetHistoryById(id);
        public bool HistoryExists(int id) => _historyRepository.HistoryExists(id);
    }
}