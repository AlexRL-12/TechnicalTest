namespace TechnicalTest.Models
{
  public class PositionHistory
  {
    public int EmployeeId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Employee Employee { get; set; }  // Relación con la clase Employee
  }
}
