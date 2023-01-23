using Microsoft.EntityFrameworkCore;
using Passzx.database.models;

namespace Passzx.database;

public class sqlserverdbcontext : DbContext
{
    public DbSet<account> Accounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Passxz");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<account>()
            .Property(a => a.Email)
            .IsRequired(false);

        modelBuilder.Entity<account>()
            .Property(a => a.Username)
            .IsRequired(false);

        modelBuilder.Entity<account>()
            .Property(a => a.Password)
            .IsRequired(false);
    }
}