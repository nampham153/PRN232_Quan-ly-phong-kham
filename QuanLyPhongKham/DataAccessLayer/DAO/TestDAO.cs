using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO { 

public class TestDAO
{
    private readonly ClinicDbContext _context;

    public TestDAO(ClinicDbContext context)
    {
        _context = context;
    }

    public List<Test> GetTests()
    {
        return _context.Tests.Include(t => t.TestResults).ToList();
    }

    public void SaveTest(Test t)
    {
        _context.Tests.Add(t);
        _context.SaveChanges();
    }

    public void UpdateTest(Test t)
    {
        _context.Entry(t).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void DeleteTest(Test t)
    {
        var t1 = _context.Tests.SingleOrDefault(c => c.TestId == t.TestId);
        if (t1 != null)
        {
            _context.Tests.Remove(t1);
            _context.SaveChanges();
        }
    }

    public Test GetTestById(int id)
    {
        return _context.Tests.Include(t => t.TestResults).FirstOrDefault(c => c.TestId == id);
    }
}
}