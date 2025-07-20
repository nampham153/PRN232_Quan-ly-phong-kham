using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System.Collections.Generic;

namespace DataAccessLayer.IRepository
{
    public interface ITestResultHistoryRepository
    {
        List<TestResultHistory> GetAllHistories();
        List<TestResultHistoryVM> GetAllHistoryVMs();
        List<TestResultHistory> GetHistoriesByUserId(int userId);
        List<TestResultHistory> GetHistoriesByTestResultId(int testResultId);
        bool AddHistory(TestResultHistoryVM historyVM);
        bool DeleteHistory(int id);
        TestResultHistory GetHistoryById(int id);
        bool HistoryExists(int id);
    }
}