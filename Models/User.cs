using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        [JsonIgnore] // ✅ Prevents cyclic reference
        public Kyc? Kyc { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();

        public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
    }
}
