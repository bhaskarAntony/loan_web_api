using System;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
  public class Loan
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Purpose { get; set; }
    public decimal MonthlySalary { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime AppliedTime { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedTime { get; set; }
    public DateTime? RejectedTime { get; set; }

    public int? UserId { get; set; } // Changed to nullable
    [JsonIgnore]
    public User? User { get; set; }

    public CreditCard? CreditCard { get; set; }
}
}
