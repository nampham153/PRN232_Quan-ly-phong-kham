using System;

namespace DataAccessLayer.ViewModels
{
    public class TestResultHistoryVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestResultId { get; set; }
        public string Action { get; set; }
        public DateTime ActionTime { get; set; }
        public string? Note { get; set; }
        
    }
}