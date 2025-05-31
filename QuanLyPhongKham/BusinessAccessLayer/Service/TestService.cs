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
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public List<Test> GetAllTests()
        {
            return _testRepository.GetTests();
        }

        public Test GetTestById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Test ID must be greater than 0");

            var test = _testRepository.GetTestById(id);
            if (test == null)
                throw new ArgumentException($"Test with ID {id} not found");

            return test;
        }

        public void CreateTest(TestVM testVM)
        {
            // Validation
            if (testVM == null)
                throw new ArgumentNullException(nameof(testVM));

            if (string.IsNullOrWhiteSpace(testVM.TestName))
                throw new ArgumentException("Test name is required");

            if (string.IsNullOrWhiteSpace(testVM.Description))
                throw new ArgumentException("Description is required");

            _testRepository.CreateTest(testVM);
        }

        public void UpdateTest(int id, TestVM testVM)
        {
            // Validation
            if (id <= 0)
                throw new ArgumentException("Test ID must be greater than 0");

            if (testVM == null)
                throw new ArgumentNullException(nameof(testVM));

            if (string.IsNullOrWhiteSpace(testVM.TestName))
                throw new ArgumentException("Test name is required");

            if (string.IsNullOrWhiteSpace(testVM.Description))
                throw new ArgumentException("Description is required");

            _testRepository.UpdateTest(id, testVM);
        }

        public void DeleteTest(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Test ID must be greater than 0");

            _testRepository.DeleteTest(id);
        }
    }
}
