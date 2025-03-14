using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; } // ✅ Changed to string
        public string PasswordSalt { get; set; } // ✅ Changed to string

        // Navigation Properties
        public Kyc? Kyc { get; set; }
        public ICollection<Loan>? Loans { get; set; }
        public ICollection<CreditCard>? CreditCards { get; set; }
    }
}
