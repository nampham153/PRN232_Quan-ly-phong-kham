
using DataAccessLayer.DAO;
using DataAccessLayer.IRepository;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class TestRepository : ITestRepository
    {
        private readonly TestDAO _testDao;

        public TestRepository(TestDAO testDao)
        {
            _testDao = testDao;
        }

        public List<Test> GetTests()
        {
            return _testDao.GetTests();
        }

        public Test GetTestById(int id)
        {
            return _testDao.GetTestById(id);
        }

        public void CreateTest(TestVM testVM)
        {
            var test = new Test
            {
                TestName = testVM.TestName,
                Description = testVM.Description,
                TestResults = new List<TestResult>()
            };

            _testDao.SaveTest(test);
        }

        public void UpdateTest(int id, TestVM testVM)
        {
            var existingTest = _testDao.GetTestById(id);
            if (existingTest == null)
                throw new ArgumentException($"Test with ID {id} not found");

            existingTest.TestName = testVM.TestName;
            existingTest.Description = testVM.Description;

            _testDao.UpdateTest(existingTest);
        }

        public void DeleteTest(int id)
        {
            var existingTest = _testDao.GetTestById(id);
            if (existingTest == null)
                throw new ArgumentException($"Test with ID {id} not found");

            _testDao.DeleteTest(existingTest);
        }
    }
}
