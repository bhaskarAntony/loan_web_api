using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        // ✅ Added: Get credit card by loan ID
        public async Task<CreditCard> GetCreditCardByLoanId(int loanId)
        {
            return await _context.CreditCards
                .Include(c => c.User) // Include User for consistency
                .FirstOrDefaultAsync(c => c.LoanId == loanId);
        }

        public async Task<CreditCard> GetCreditCardById(int id)
        {
            return await _context.CreditCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CreditCard> GetCreditCardByNumberAsync(string cardNumber)
        {
            return await _context.CreditCards
                .Include(c => c.User) // Ensure User is loaded
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
        }

        // ✅ Added: Get all credit cards
        public async Task<IEnumerable<CreditCard>> GetAllCreditCards()
        {
            return await _context.CreditCards
                .Include(c => c.User) // Include User for consistency
                .ToListAsync();
        }

        public async Task<List<CreditCardTransaction>> GetTransactionsByCardNumber(string cardNumber)
        {
            return await _context.CreditCardTransactions
                .Where(t => t.SenderCardNumber == cardNumber || t.ReceiverCardNumber == cardNumber)
                .ToListAsync();
        }

        public async Task AddTransaction(CreditCardTransaction transaction)
        {
            await _context.CreditCardTransactions.AddAsync(transaction);
        }

        public async Task AddCreditCard(CreditCard creditCard)
        {
            await _context.CreditCards.AddAsync(creditCard);
        }

        public void DeleteCreditCard(CreditCard creditCard)
        {
            _context.CreditCards.Remove(creditCard);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}