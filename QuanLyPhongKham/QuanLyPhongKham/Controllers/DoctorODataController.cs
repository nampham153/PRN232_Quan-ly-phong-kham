using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Results;

namespace QuanLyPhongKham.Controllers
{
    [Route("odata/[controller]")]
    public class DoctorsController : ODataController
    {
        private readonly ClinicDbContext _context;

        public DoctorsController(ClinicDbContext context)
        {
            _context = context;
        }

        [HttpGet]  // GET odata/Doctors
        [EnableQuery]
        public IQueryable<User> Get()
        {
            return _context.Users
                .Include(u => u.Account)
                    .ThenInclude(a => a.Role)
                .Where(u => u.Account.Role.RoleId == 2);
        }

        [HttpGet("({key})")] 
        [EnableQuery]
        public SingleResult<User> Get([FromODataUri] int key)
        {
            var result = _context.Users
                .Include(u => u.Account)
                    .ThenInclude(a => a.Role)
                .Where(u => u.UserId == key && u.Account.Role.RoleId == 2);

            return SingleResult.Create(result);
        }
    }

}
