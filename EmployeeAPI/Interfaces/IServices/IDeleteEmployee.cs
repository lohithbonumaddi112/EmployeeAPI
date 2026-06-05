using EmployeeAPI.Models;

namespace EmployeeAPI.Interfaces
{
    public interface IDeleteEmployee
    {
        Task<Employee?> DeleteEmployees(int id);
    }
}
