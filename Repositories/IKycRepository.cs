using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public interface IKycRepository
    {
        Task<IEnumerable<Kyc>> GetAllKycs(); // ✅ Get all KYC records
        Task<Kyc> GetKycById(int id); // ✅ Get KYC by ID
        Task<Kyc> GetKycByUserId(int userId); // ✅ Get KYC by User ID
        Task AddKyc(Kyc kyc); // ✅ Add a new KYC record
        Task SaveChanges(); // ✅ Save changes to the database
    }
}
