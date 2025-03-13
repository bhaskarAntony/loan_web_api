using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Data
{
    public class LoanDbContext : DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Kyc> Kycs { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Kyc)
                .WithOne(k => k.User)
                .HasForeignKey<Kyc>(k => k.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Loan>()
                .HasOne(l => l.CreditCard)
                .WithOne(c => c.Loan)
                .HasForeignKey<CreditCard>(c => c.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CreditCard>()
                .Property(c => c.ExpiryDate)
                .HasColumnType("datetime");

            modelBuilder.Entity<Loan>()
                .Property(l => l.AppliedTime)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Kyc>()
                .Property(k => k.AppliedTime)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
