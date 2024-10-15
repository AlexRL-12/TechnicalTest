using Microsoft.EntityFrameworkCore;
using TechnicalTest.Models;
using TechnicalTest.Services;

public class EmployeeService : IEmployeeService
{
  private readonly AppDbContext _context;

  public EmployeeService(AppDbContext context)
  {
    _context = context;
  }

  public IEnumerable<Employee> GetAllEmployees()
  {
    return _context.Employees.AsNoTracking().ToList();
  }

  public Employee GetEmployeeById(int id)
  {
    return _context.Employees.AsNoTracking().FirstOrDefault(e => e.Id == id);
  }

  public void AddEmployee(Employee employee)
  {
    _context.Employees.Add(employee);
    _context.SaveChanges();
  }

  public Employee UpdateEmployee(int id, Employee employee)
  {
    var existingEmployee = _context.Employees.Find(id);
    if (existingEmployee == null) return null;

    existingEmployee.Name = employee.Name;
    existingEmployee.Position = employee.Position;
    // Otros campos a actualizar

    _context.Employees.Update(existingEmployee);
    _context.SaveChanges();
    return existingEmployee;
  }

  public bool DeleteEmployee(int id)
  {
    var employee = _context.Employees.Find(id);
    if (employee == null) return false;

    _context.Employees.Remove(employee);
    _context.SaveChanges();
    return true;
  }
}
