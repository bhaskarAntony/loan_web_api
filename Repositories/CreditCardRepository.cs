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

        public async Task<List<CreditCard>> GetCreditCardsByUserId(int userId)
        {
            return await _context.CreditCards
                .Where(c => c.Loan.UserId == userId)
                .ToListAsync();  // ✅ Ensure it returns a List
        }

        public async Task<CreditCard> GetCreditCardById(int id)
        {
            return await _context.CreditCards
                .Include(c => c.Loan)
                .FirstOrDefaultAsync(c => c.Id == id);
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
