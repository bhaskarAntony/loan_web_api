using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LoanDbContext _context;

        public LoanRepository(LoanDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllLoans()
        {
            return await _context.Loans.ToListAsync();
        }

        public async Task<Loan> GetLoanById(int id)
        {
            return await _context.Loans.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddLoan(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        public void DeleteLoan(Loan loan)
        {
            _context.Loans.Remove(loan);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
