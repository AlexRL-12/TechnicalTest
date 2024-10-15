using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TechnicalTest.Models;

public class AppDbContext : DbContext
{
  public DbSet<Employee> Employees { get; set; }
  public DbSet<Department> Departments { get; set; }
  public DbSet<Project> Projects { get; set; }
  public DbSet<PositionHistory> PositionHistories { get; set; }

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Employee>()
        .HasMany(e => e.PositionHistories)
        .WithOne(p => p.Employee);

    modelBuilder.Entity<Employee>()
        .HasMany(e => e.Projects)
        .WithMany(p => p.Employees);
  }
}
