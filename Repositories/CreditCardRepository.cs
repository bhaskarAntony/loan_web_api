using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly LoanDbContext _context;

        public CreditCardRepository(LoanDbContext context)
        {
            _context = context;
        }

        // ✅ Get Credit Card by Loan ID
        public async Task<CreditCard> GetCreditCardByLoanId(int loanId)
        {
            return await _context.CreditCards
                .Include(cc => cc.Loan)
                .FirstOrDefaultAsync(cc => cc.LoanId == loanId);
        }

        // ✅ Get Credit Card by Number for Uniqueness Check
        public async Task<CreditCard> GetCreditCardByNumberAsync(string cardNumber)
        {
            return await _context.CreditCards
                .FirstOrDefaultAsync(cc => cc.CardNumber == cardNumber);
        }

        // ✅ Get Credit Card by ID (NEW)
        public async Task<CreditCard> GetCreditCardById(int id)
        {
            return await _context.CreditCards
                .Include(cc => cc.Loan)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        // ✅ Add a New Credit Card
        public async Task AddCreditCard(CreditCard creditCard)
        {
            await _context.CreditCards.AddAsync(creditCard);
        }

        // ✅ Delete Credit Card (NEW)
        public void DeleteCreditCard(CreditCard creditCard)
        {
            _context.CreditCards.Remove(creditCard);
        }

        // ✅ Save Changes
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
