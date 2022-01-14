using Gabriel.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gabriel.Data
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option) : base(option)
        {
                    
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<AppUser> ApplicationUsers { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
    }
}
