using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class TestDAO
    {
        public static List<Test> GetTest()
        {
            var listTests = new List<Test>();
            try
            {
                using var context = new ClinicDbContext();
                listTests = context.Tests.Include(t => t.TestResults).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listTests;
        }

        public static void SaveTest(Test t)
        {
            try
            {
                using var context = new ClinicDbContext();
                context.Tests.Add(t);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateTest(Test t)
        {
            try
            {
                using var context = new ClinicDbContext();
                context.Entry<Test>(t).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DeleteTest(Test t)
        {
            try
            {
                using var context = new ClinicDbContext();
                var t1 = context.Tests.SingleOrDefault(c => c.TestId == t.TestId);
                if (t1 != null)
                {
                    context.Tests.Remove(t1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Test GetTestById(int id)
        {
            try
            {
                using var db = new ClinicDbContext();
                return db.Tests.Include(t => t.TestResults).FirstOrDefault(c => c.TestId.Equals(id));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
