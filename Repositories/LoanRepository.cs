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

        // ✅ Get All Loans
        // public async Task<IEnumerable<Loan>> GetAllLoans()
        // {
        //     return await _context.Loans.ToListAsync();
        // }

        public async Task<IEnumerable<Loan>> GetAllLoans()
        {
            return await _context.Loans
                .Include(l => l.User) // Include User for mapping
                .ToListAsync();
        }
        

        // ✅ Get Loan by ID
        public async Task<Loan> GetLoanById(int id)
        {
            return await _context.Loans
                .Include(l => l.User) // Ensure User relationship is included
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        // ✅ Get Loan by User ID (NEW)
        public async Task<Loan> GetLoanByUserId(int userId)
        {
            return await _context.Loans
                .Include(l => l.User) // Include related User data
                .FirstOrDefaultAsync(l => l.UserId == userId);
        }

        // ✅ Add Loan
        public async Task AddLoan(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        // ✅ Delete Loan
        public void DeleteLoan(Loan loan)
        {
            _context.Loans.Remove(loan);
        }

        public async Task UpdateLoan(Loan loan)
        {
            _context.Loans.Update(loan); // Explicitly mark as modified (optional, since tracked)
        }

        // ✅ Save Changes with Improved Logic
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
