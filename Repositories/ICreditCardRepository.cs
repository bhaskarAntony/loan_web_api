using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface ICreditCardRepository
    {
        Task<List<CreditCard>> GetCreditCardsByUserId(int userId);  // ✅ Correct Method Name
        Task<CreditCard> GetCreditCardById(int id);
        Task AddCreditCard(CreditCard creditCard);
        void DeleteCreditCard(CreditCard creditCard);
        Task<bool> SaveChanges();
    }
}
