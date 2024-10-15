using Microsoft.EntityFrameworkCore;
using TechnicalTest.Models;
using TechnicalTest.Services;
using System;

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
    // Input validations
    if (string.IsNullOrWhiteSpace(employee.Name))
    {
      throw new ArgumentException("Employee name is required.");
    }

    if (employee.Salary <= 0)
    {
      throw new ArgumentOutOfRangeException("Salary must be a positive value.");
    }

    var existingEmployee = _context.Employees
        .FirstOrDefault(e => e.Id == employee.Id || e.Name == employee.Name);

    if (existingEmployee != null)
    {
      throw new InvalidOperationException("The employee already exists.");
    }

    // Add the new employee
    _context.Employees.Add(employee);

    // Error handling when saving changes
    try
    {
      _context.SaveChanges();
    }
    catch (DbUpdateException ex)
    {
      throw new Exception("An error occurred while saving changes to the database.", ex);
    }
  }

  public Employee UpdateEmployee(int id, Employee employee)
  {
    // Check if the employee exists
    var existingEmployee = _context.Employees.Find(id);
    if (existingEmployee == null)
    {
      throw new InvalidOperationException("The employee does not exist.");
    }

    // Input validations
    if (string.IsNullOrWhiteSpace(employee.Name))
    {
      throw new ArgumentException("Employee name is required.");
    }

    if (employee.Salary <= 0)
    {
      throw new ArgumentOutOfRangeException("Salary must be a positive value.");
    }

    // Update employee properties
    existingEmployee.Name = employee.Name;
    existingEmployee.Position = employee.Position;

    if (!string.IsNullOrWhiteSpace(employee.PasswordHash))
    {
      existingEmployee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(employee.PasswordHash);
    }

    // Error handling when saving changes
    try
    {
      _context.SaveChanges();
    }
    catch (DbUpdateException ex)
    {
      throw new Exception("An error occurred while saving changes to the database.", ex);
    }

    return existingEmployee;
  }

  public bool DeleteEmployee(int id)
  {
    // Check if the employee exists
    var employee = _context.Employees.Find(id);
    if (employee == null)
    {
      throw new InvalidOperationException("The employee does not exist.");
    }

    // Remove the employee
    _context.Employees.Remove(employee);

    // Error handling when saving changes
    try
    {
      _context.SaveChanges();
    }
    catch (DbUpdateException ex)
    {
      throw new Exception("An error occurred while deleting the employee.", ex);
    }

    return true;
  }
}
