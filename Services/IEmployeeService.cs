using TechnicalTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechnicalTest.Services
{
  public interface IEmployeeService
  {
    IEnumerable<Employee> GetAllEmployees();
    Employee GetEmployeeById(int id);
    Task AddEmployee(Employee employee); 
    Employee UpdateEmployee(int id, Employee updatedEmployee);
    bool DeleteEmployee(int id);
  }
}
