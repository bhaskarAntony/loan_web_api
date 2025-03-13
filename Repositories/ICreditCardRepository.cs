using LoanManagementSystem.Models;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface ICreditCardRepository
    {
        Task<CreditCard> GetCreditCardByLoanId(int loanId);   // Get Credit Card by Loan ID
        Task<CreditCard> GetCreditCardByNumberAsync(string cardNumber);  // Get Credit Card by Number
        Task<CreditCard> GetCreditCardById(int id);            // 🔹 New Method: Get Credit Card by ID
        Task AddCreditCard(CreditCard creditCard);
        void DeleteCreditCard(CreditCard creditCard);          // 🔹 New Method: Delete Credit Card
        Task<bool> SaveChanges();
    }
}
