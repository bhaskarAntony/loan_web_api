using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanManagementSystem.Repositories
{
    public class KycRepository : IKycRepository
    {
        private readonly LoanDbContext _context;

        public KycRepository(LoanDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kyc>> GetAllKycs()
        {
            return await _context.Kycs.ToListAsync();
        }

        public async Task<Kyc> GetKycById(int id)
        {
            return await _context.Kycs.FindAsync(id);
        }

        public async Task<Kyc> GetKycByUserId(int userId)
        {
            return await _context.Kycs.FirstOrDefaultAsync(k => k.UserId == userId);
        }

        public async Task AddKyc(Kyc kyc)
        {
            await _context.Kycs.AddAsync(kyc);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
