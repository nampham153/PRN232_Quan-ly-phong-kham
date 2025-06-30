using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DataAccessLayer.models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public bool Status { get; set; }
        public Role Role { get; set; }

        public RefreshToken RefreshToken { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public Patient Patient { get; set; }
    }
}
