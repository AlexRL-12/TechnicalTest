using TechnicalTest.Models;  

namespace TechnicalTest.Services  
{
  public interface IEmployeeService
  {
    IEnumerable<Employee> GetAllEmployees();
    Employee GetEmployeeById(int id);
    void AddEmployee(Employee employee);
    Employee UpdateEmployee(int id, Employee updatedEmployee);
    bool DeleteEmployee(int id);
  }
}
