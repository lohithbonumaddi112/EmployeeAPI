using EmployeeAPI.Interfaces.IServices;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;



namespace EmployeeAPI.Services
{
    public class CreateEmployee : ICreateEmployee
    {
        
        private readonly DbemployeeContext _context;
        public CreateEmployee(DbemployeeContext context)
        {
            _context = context;
        }
        public async Task<Employee> CreateEmploye(Employee employee)
        {
            
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    
    }
}
