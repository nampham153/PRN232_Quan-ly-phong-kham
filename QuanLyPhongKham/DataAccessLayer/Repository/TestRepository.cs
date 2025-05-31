
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
        public List<Test> GetAllTests()
        {
            return TestDAO.GetTest();
        }

        public Test GetTestById(int id)
        {
            return TestDAO.GetTestById(id);
        }

        public void CreateTest(TestVM testVM)
        {
            var test = new Test
            {
                TestName = testVM.TestName,
                Description = testVM.Description,
                TestResults = new List<TestResult>()
            };

            TestDAO.SaveTest(test);
        }

        public void UpdateTest(int id, TestVM testVM)
        {
            var existingTest = TestDAO.GetTestById(id);
            if (existingTest == null)
                throw new ArgumentException($"Test with ID {id} not found");

            existingTest.TestName = testVM.TestName;
            existingTest.Description = testVM.Description;

            TestDAO.UpdateTest(existingTest);
        }

        public void DeleteTest(int id)
        {
            var existingTest = TestDAO.GetTestById(id);
            if (existingTest == null)
                throw new ArgumentException($"Test with ID {id} not found");

            TestDAO.DeleteTest(existingTest);
        }
    }
}
