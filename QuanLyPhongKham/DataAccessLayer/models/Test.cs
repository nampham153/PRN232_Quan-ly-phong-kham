using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.models
{
    public class Test
    {
        [Key]
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string Description { get; set; }

        public ICollection<TestResult> TestResults { get; set; }
    }
}
