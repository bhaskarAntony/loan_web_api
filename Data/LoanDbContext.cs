using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Data
{
    public class LoanDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Kyc> Kycs { get; set; }
        public DbSet<CreditCardTransaction> CreditCardTransactions { get; set; }

        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User -> Kyc (One-to-One, Cascade Delete)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Kyc)
                .WithOne(k => k.User)
                .HasForeignKey<Kyc>(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); // Kyc requires a User

            // User -> Loans (One-to-Many, Restrict Delete)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Loans)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(); // Loan requires a User

            // User -> CreditCards (One-to-Many, Restrict Delete)
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreditCards)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(); // CreditCard requires a User

            // Loan -> CreditCard (One-to-One, Cascade Delete)
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.CreditCard)
                .WithOne(c => c.Loan)
                .HasForeignKey<CreditCard>(c => c.LoanId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false); // LoanId is optional in CreditCard

            // CreditCardTransaction - Default value for TransactionDate
            modelBuilder.Entity<CreditCardTransaction>()
                .Property(t => t.TransactionDate)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}