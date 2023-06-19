using Microsoft.EntityFrameworkCore;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Persistence.Context;

public class SearchJobAppDbContext : DbContext
{
    public DbSet<Employer> Employers { get; set; }
    public DbSet<Post> Posts { get; set; }

    public SearchJobAppDbContext(DbContextOptions<SearchJobAppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employer>().HasKey(p => p.Id);
        modelBuilder.Entity<Employer>().Property(p => p.Password).IsRequired();
        modelBuilder.Entity<Employer>().Property(p => p.Email).IsRequired();
        modelBuilder.Entity<Employer>().Property(p => p.Title).IsRequired();
        modelBuilder.Entity<Employer>().Property(p => p.Phone).IsRequired();
        modelBuilder.Entity<Employer>().Property(p => p.Address).IsRequired();
        modelBuilder.Entity<Employer>().Property(p => p.RemainingPostingQuantity).HasDefaultValue(2);

        modelBuilder.Entity<Post>().HasKey(p => p.Id);
        modelBuilder.Entity<Post>().Property(p => p.Title).IsRequired();
        modelBuilder.Entity<Post>().Ignore(p => p.EmployerTitle);
        modelBuilder.Entity<Post>().Property(p => p.Message).IsRequired();
        modelBuilder.Entity<Post>().Property(p => p.StartDate).IsRequired();
        modelBuilder.Entity<Post>().Property(p => p.EndDate).IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}