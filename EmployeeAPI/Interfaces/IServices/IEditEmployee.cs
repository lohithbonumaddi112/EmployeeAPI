using EmployeeAPI.Models;
namespace EmployeeAPI.Interfaces
{
    public interface IEditEmployee
    {
        Task<Employee?> EditEmployee(int id, Employee employee);
    }
}
