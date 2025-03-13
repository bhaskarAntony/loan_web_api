using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }  // ✅ Change to DateTime
        public string CVV { get; set; }
        public decimal ApprovedAmount { get; set; }

        public int LoanId { get; set; }

        [JsonIgnore] // ✅ Prevents cyclic reference
        public Loan? Loan { get; set; }
    }
}