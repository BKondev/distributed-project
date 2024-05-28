using Microsoft.EntityFrameworkCore;
using ManagementApi.Entities;
using System;

namespace ManagementApi.Database
{
    public class DBManagementContext: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }
  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Database=managementapplicationdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trusted_Connection = true");
        }
    }
}