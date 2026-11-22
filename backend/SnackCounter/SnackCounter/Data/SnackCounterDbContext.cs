using Microsoft.EntityFrameworkCore;
using SnackCounter.Models.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SnackCounter.Data
{
    public class SnackCounterDbContext : DbContext
    {
        public SnackCounterDbContext(
            DbContextOptions<SnackCounterDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Refill> Refills => Set<Refill>();

        public DbSet<Purchase> Purchases => Set<Purchase>();

        public DbSet<Reimbursement> Reimbursements => Set<Reimbursement>();

        public DbSet<OfficeBudget> OfficeBudgets => Set<OfficeBudget>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeId)
                .IsUnique();

            modelBuilder.Entity<OfficeBudget>()
                .HasIndex(b => b.Office)
                .IsUnique();

            modelBuilder.Entity<Refill>()
                .HasOne(r => r.AssignedEmployee)
                .WithMany(e => e.AssignedRefills)
                .HasForeignKey(r => r.AssignedEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Refill>()
                .HasOne(r => r.Purchase)
                .WithOne(p => p.Refill)
                .HasForeignKey<Purchase>(p => p.RefillId);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Reimbursement)
                .WithOne(r => r.Purchase)
                .HasForeignKey<Reimbursement>(r => r.PurchaseId);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OfficeBudget>()
                .Property(b => b.MonthlyBudget)
                .HasPrecision(18, 2);
        }
    }
}