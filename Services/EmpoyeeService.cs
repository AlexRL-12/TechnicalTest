using Microsoft.EntityFrameworkCore;
using TechnicalTest.Models;
using TechnicalTest.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

  public async Task AddEmployee(Employee employee)
  {
    if (string.IsNullOrWhiteSpace(employee.Name))
    {
      throw new ArgumentException("Employee name is required.");
    }

    if (employee.Salary <= 0)
    {
      throw new ArgumentOutOfRangeException("Salary must be a positive value.");
    }

    var existingEmployee = await _context.Employees
        .AsNoTracking()
        .FirstOrDefaultAsync(e => e.Name == employee.Name);

    if (existingEmployee != null)
    {
      throw new InvalidOperationException("An employee with this name already exists.");
    }

    await _context.Employees.AddAsync(employee);

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
      throw new Exception("An error occurred while saving changes to the database.", ex);
    }
  }

  public Employee UpdateEmployee(int id, Employee employee)
  {
    var existingEmployee = _context.Employees.Find(id);

    if (existingEmployee == null)
    {
      throw new InvalidOperationException("The employee does not exist.");
    }

    if (string.IsNullOrWhiteSpace(employee.Name))
    {
      throw new ArgumentException("Employee name is required.");
    }

    if (employee.Salary <= 0)
    {
      throw new ArgumentOutOfRangeException("Salary must be a positive value.");
    }

    existingEmployee.Name = employee.Name;
    existingEmployee.Position = employee.Position;
    existingEmployee.Salary = employee.Salary;

    if (!string.IsNullOrWhiteSpace(employee.PasswordHash))
    {
      existingEmployee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(employee.PasswordHash);
    }

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
    var employee = _context.Employees.Find(id);
    if (employee == null)
    {
      throw new InvalidOperationException("The employee does not exist.");
    }

    _context.Employees.Remove(employee);

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
