using TechnicalTest.Models;

public class Employee
{
  public int Id { get; set; }
  public string Name { get; set; }
  public int CurrentPosition { get; set; }
  public decimal Salary { get; set; }

  public string Position { get; set; }
  public int DepartmentId { get; set; }

  public string PasswordHash { get; set; }

  public List<PositionHistory> PositionHistories { get; set; } = new List<PositionHistory>();
  public List<Project> Projects { get; set; } = new List<Project>();

  public decimal CalculateBonus()
  {
    return CurrentPosition > 1 ? Salary * 0.20M : Salary * 0.10M;
  }
}
