using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult GetEmployees()
    {
      var employees = _employeeService.GetAllEmployees();
      return Ok(employees);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public IActionResult GetEmployee(int id)
    {
      var employee = _employeeService.GetEmployeeById(id);
      if (employee == null) return NotFound();
      return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AddEmployee(Employee employee)
    {
      _employeeService.AddEmployee(employee);
      return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateEmployee(int id, Employee employee)
    {
      var updatedEmployee = _employeeService.UpdateEmployee(id, employee);
      if (updatedEmployee == null) return NotFound();
      return Ok(updatedEmployee);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteEmployee(int id)
    {
      var success = _employeeService.DeleteEmployee(id);
      if (!success) return NotFound();
      return NoContent();
    }
  }
}
