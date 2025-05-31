using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.IService
{
    public interface ITestService
    {
        List<Test> GetAllTests();
        Test GetTestById(int id);
        void CreateTest(TestVM testVM);
        void UpdateTest(int id, TestVM testVM);
        void DeleteTest(int id);
    }
}
