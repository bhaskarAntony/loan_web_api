using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllLoans();
        Task<Loan> GetLoanById(int id);
        Task<Loan> GetLoanByUserId(int userId); // New method for fetching by UserId
        Task AddLoan(Loan loan);
        void DeleteLoan(Loan loan);
        Task<bool> SaveChanges(); // Updated to return Task<bool>
    }
}
