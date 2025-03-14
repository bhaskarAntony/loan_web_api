using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface ICreditCardRepository
    {
        Task<CreditCard> GetCreditCardByLoanId(int loanId);
        Task<CreditCard> GetCreditCardById(int id);
        Task<CreditCard> GetCreditCardByNumberAsync(string cardNumber);
        Task<IEnumerable<CreditCard>> GetAllCreditCards();
        Task<List<CreditCardTransaction>> GetTransactionsByCardNumber(string cardNumber);
        Task AddTransaction(CreditCardTransaction transaction);
        Task AddCreditCard(CreditCard creditCard);
        void DeleteCreditCard(CreditCard creditCard);
        Task<bool> SaveChanges();
    }
}