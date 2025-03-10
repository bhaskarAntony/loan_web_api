using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllLoans();
        Task<Loan> GetLoanById(int id);
        Task AddLoan(Loan loan);
        void DeleteLoan(Loan loan);
        Task<bool> SaveChanges();
    }
}
