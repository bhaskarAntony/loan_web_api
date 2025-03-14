// File: LoanManagementSystem/Models/DTOs.cs
namespace LoanManagementSystem.Models
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<LoanDto> Loans { get; set; } = new List<LoanDto>();
        public List<CreditCardDto> CreditCards { get; set; } = new List<CreditCardDto>();
    }

    public class LoanDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public decimal MonthlySalary { get; set; }
        public string Status { get; set; }
        public DateTime AppliedTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime? RejectedTime { get; set; }
        public CreditCardDto CreditCard { get; set; } // Optional, no User reference
    }
    public class LoanResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Keep as non-nullable int
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public decimal MonthlySalary { get; set; }
        public string Status { get; set; }
        public DateTime AppliedTime { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public DateTime? RejectedTime { get; set; }
    }

    public class LoanRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public decimal MonthlySalary { get; set; }
    }

    public class CreditCardDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CVV { get; set; }
        public decimal ApprovedAmount { get; set; }
        public int? LoanId { get; set; } // Optional, no User or Loan navigation
    }

    // Existing DTOs (for completeness)
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}