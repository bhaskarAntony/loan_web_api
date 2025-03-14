using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
   public class CreditCard
{
    public int Id { get; set; }
    public string CardNumber { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string CVV { get; set; }
    public decimal ApprovedAmount { get; set; }

    public int? LoanId { get; set; } // Already nullable
    [JsonIgnore]
    public Loan? Loan { get; set; }

    public int? UserId { get; set; } // Changed to nullable
    [ForeignKey("UserId")]
    public User? User { get; set; }
}
}
