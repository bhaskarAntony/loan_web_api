using System;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Models
{
    public class CreditCardTransaction
    {
        [Key]
        public int Id { get; set; }

        public string SenderCardNumber { get; set; }
        public string ReceiverCardNumber { get; set; }

        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
