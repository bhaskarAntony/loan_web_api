namespace LoanManagementSystem.Models
{
    public class CreditTransferRequest
    {
        public string SenderCardNumber { get; set; }
        public string ReceiverCardNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
