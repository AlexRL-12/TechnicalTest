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

  // Método que calcula el bono
  public decimal CalculateBonus()
  {
    // El bono es del 20% si es un gerente (CurrentPosition > 1), del 10% si es un empleado regular
    return CurrentPosition > 1 ? Salary * 0.20M : Salary * 0.10M;
  }

  // Propiedad calculada que retorna el salario total con el bono
  public decimal TotalSalary => Salary + CalculateBonus();
}
