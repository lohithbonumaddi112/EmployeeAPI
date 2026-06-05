using EmployeeAPI.Models;

namespace EmployeeAPI.Interfaces.IServices
{
    public interface ICreateEmployee
    {
        public Task<Employee> CreateEmploye(Employee employee);
    }
}
