using System;

namespace DataAccessLayer.models
{
    public class TestResultHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TestResultId { get; set; }
        public string Action { get; set; } // Create, Update, Delete, View
        public DateTime ActionTime { get; set; }
        public string? Note { get; set; } // Optional: thêm ghi chú nếu cần
    }
}