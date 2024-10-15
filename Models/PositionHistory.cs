using System.ComponentModel.DataAnnotations;

namespace TechnicalTest.Models
{
  public class PositionHistory
  {
    [Key] 
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Employee Employee { get; set; } 
  }
}
