using System.ComponentModel.DataAnnotations;
using TechnicalTest.Models;

public class Department
{
  public int Id { get; set; }
  [MaxLength(255)]
  public string Name { get; set; }

  public List<Employee> Employees { get; set; } = new List<Employee>();
}
