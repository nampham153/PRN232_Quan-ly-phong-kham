using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.ViewModels
{
    public class TestSummaryVM
    {
        public DateTime TestDate { get; set; }
        public List<TestResultVM> Results { get; set; }
    }
}
