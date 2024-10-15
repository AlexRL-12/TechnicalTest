using TechnicalTest.Models;  // Cambiar esto a technicalTest.Models si es necesario

namespace TechnicalTest.Services  // Asegúrate de que el namespace sea consistente
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
