using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);  // ✅ Add this method
        Task AddUser(User user);
        void DeleteUser(User user);
        Task<bool> SaveChanges();
    }
}
