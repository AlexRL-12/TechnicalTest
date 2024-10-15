namespace TechnicalTest.Models
{
  public class Employee
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int CurrentPosition { get; set; }
    public decimal Salary { get; set; }

    public string Position { get; set; } // Asegúrate de que esta propiedad exista

    // Lista de posiciones pasadas
    public List<PositionHistory> PositionHistories { get; set; } = new List<PositionHistory>();
    public List<Project> Projects { get; set; } = new List<Project>();

    public decimal CalculateBonus()
    {
      // Asumimos que posiciones con id mayor a 1 son "managers"
      return CurrentPosition > 1 ? Salary * 0.20M : Salary * 0.10M;
    }
  }
}
