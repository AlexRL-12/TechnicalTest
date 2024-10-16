using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TechnicalTest.Models;
using TechnicalTest.Services;

namespace TechnicalTest.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class EmployeesController : ControllerBase
  {
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
      _employeeService = employeeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public ActionResult<IEnumerable<Employee>> GetEmployees()
    {
      var employees = _employeeService.GetAllEmployees();
      return Ok(employees);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public ActionResult<Employee> GetEmployee(int id)
    {
      var employee = _employeeService.GetEmployeeById(id);
      if (employee == null)
        return NotFound($"Employee with ID {id} not found.");

      return Ok(new
      {
        employee.Id,
        employee.Name,
        employee.Position,
        employee.CurrentPosition,
        employee.Salary,
        TotalSalary = employee.TotalSalary,
        employee.DepartmentId
      });
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<Employee> AddEmployee([FromBody] Employee employee)
    {
      if (employee == null)
      {
        return BadRequest("Employee data is required.");
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var existingEmployee = _employeeService.GetAllEmployees().FirstOrDefault(e => e.Name == employee.Name);
      if (existingEmployee != null)
      {
        return Conflict("An employee with this name already exists.");
      }

      _employeeService.AddEmployee(employee);
      return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee employee)
    {
      if (employee == null)
      {
        return BadRequest("Employee data is required.");
      }

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var updatedEmployee = _employeeService.UpdateEmployee(id, employee);
      if (updatedEmployee == null) return NotFound($"Employee with ID {id} not found.");
      return Ok(updatedEmployee);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteEmployee(int id)
    {
      var success = _employeeService.DeleteEmployee(id);
      if (!success) return NotFound($"Employee with ID {id} not found.");
      return NoContent();
    }
  }
}
