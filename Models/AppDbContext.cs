using Microsoft.EntityFrameworkCore;
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
        .Property(e => e.Salary)
        .HasColumnType("decimal(18,2)");

    modelBuilder.Entity<Employee>()
        .HasMany(e => e.PositionHistories)
        .WithOne(p => p.Employee)
        .HasForeignKey(p => p.EmployeeId);

    modelBuilder.Entity<Employee>()
        .HasMany(e => e.Projects)
        .WithMany(p => p.Employees);

    modelBuilder.Entity<Department>()
        .Property(d => d.Name)
        .HasColumnType("varchar(100)")
        .IsRequired();

    modelBuilder.Entity<Department>()
        .HasMany(d => d.Employees)
        .WithOne()
        .HasForeignKey(e => e.DepartmentId);

    modelBuilder.Entity<PositionHistory>()
        .HasKey(ph => ph.Id);


    modelBuilder.Entity<Project>()
        .HasKey(p => p.Id);

    SeedData(modelBuilder);
  }

  private void SeedData(ModelBuilder modelBuilder)
  {
    // Crear Departamentos
    modelBuilder.Entity<Department>().HasData(
        new Department { Id = 1, Name = "IT" },
        new Department { Id = 2, Name = "HR" },
        new Department { Id = 3, Name = "Finance" }
    );

    // Crear Proyectos
    modelBuilder.Entity<Project>().HasData(
        new Project { Id = 1, Name = "Project A" },
        new Project { Id = 2, Name = "Project B" },
        new Project { Id = 3, Name = "Project C" }
    );

    // Crear Empleados (añadiendo PasswordHash)
    modelBuilder.Entity<Employee>().HasData(
        new Employee { Id = 1, Name = "Alice Johnson", CurrentPosition = 2, Salary = 60000, Position = "Software Engineer", DepartmentId = 1, PasswordHash = "alicepass" },
        new Employee { Id = 2, Name = "Bob Smith", CurrentPosition = 1, Salary = 50000, Position = "HR Specialist", DepartmentId = 2, PasswordHash = "bobpass" },
        new Employee { Id = 3, Name = "Charlie Brown", CurrentPosition = 3, Salary = 80000, Position = "Financial Analyst", DepartmentId = 3, PasswordHash = "charliepass" },
        new Employee { Id = 4, Name = "Diana Prince", CurrentPosition = 1, Salary = 70000, Position = "Project Manager", DepartmentId = 1, PasswordHash = "dianapass" }
    );

    // Crear Historias de Posición para los empleados
    modelBuilder.Entity<PositionHistory>().HasData(
        new PositionHistory { Id = 1, EmployeeId = 1, Position = "Junior Developer", StartDate = DateTime.Now.AddYears(-2), EndDate = DateTime.Now.AddYears(-1) },
        new PositionHistory { Id = 2, EmployeeId = 2, Position = "Recruiter", StartDate = DateTime.Now.AddYears(-3), EndDate = DateTime.Now.AddYears(-1) },
        new PositionHistory { Id = 3, EmployeeId = 3, Position = "Intern", StartDate = DateTime.Now.AddYears(-4), EndDate = DateTime.Now.AddYears(-3) }
    );

    // Relacionar empleados con proyectos
    modelBuilder.Entity<Project>()
        .HasMany(p => p.Employees)
        .WithMany(e => e.Projects)
        .UsingEntity(j => j
            .HasData(
                new { ProjectsId = 1, EmployeesId = 1 }, // Alice en Project A
                new { ProjectsId = 2, EmployeesId = 1 }, // Alice en Project B
                new { ProjectsId = 3, EmployeesId = 2 }, // Bob en Project C
                new { ProjectsId = 1, EmployeesId = 3 }  // Charlie en Project A
            ));
  }

}