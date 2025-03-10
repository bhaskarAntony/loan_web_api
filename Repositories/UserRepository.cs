using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LoanDbContext _context;

        public UserRepository(LoanDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Loans)
                .ThenInclude(l => l.CreditCard)
                .Include(u => u.CreditCards)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users
                .Include(u => u.Loans)
                .ThenInclude(l => l.CreditCard)
                .Include(u => u.CreditCards)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetUserByEmail(string email)  // ✅ Add this method
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
