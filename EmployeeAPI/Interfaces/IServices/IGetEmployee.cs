using EmployeeAPI.Dto_s;
using EmployeeAPI.Models;
namespace EmployeeAPI.Interfaces
{
    public interface IGetEmployee
    {
        Task<List<Employee>> GetEmployees();
        Task<List<EmployeeDto>> SalaryByRange(decimal minSalary, decimal maxSalary);
        Task<EmployeeDto?> GetEmployeesByName(string name);
        Task<EmployeeDto?> GetEmployeById(int Id);
        Task<bool> CheckSalary();
        Task<int> CheckMinSalary(decimal minSalary);
        Task<List<EmployeeDto>> SortEmployeesBySalary();
        Task<List<EmployeeDepartmentDto>> GetEmployeesWithDepartment();
        Task<List<DepartmentEmployeesDto>> GetDepartmentsWithEmployees();
        Task<List<EmployeeDepartmentDto>> GetDptsWithEmployee();
        Task<List<EmployeeDepartmentDto>> GetEmployeeDepartmentById(int id);
    }
}
