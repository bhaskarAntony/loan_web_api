using System;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public class Kyc
    {
        public int Id { get; set; }
        public string AadharNumber { get; set; }
        public string PanNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Status { get; set; } = "Pending";

        public DateTime? AppliedTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime? RejectedTime { get; set; }

        public int UserId { get; set; }

        [JsonIgnore] // ✅ Prevents cyclic reference
        public User? User { get; set; }
    }
}
