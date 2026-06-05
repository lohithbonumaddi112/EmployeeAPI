using EmployeeAPI.Interfaces;
using EmployeeAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Services
{
    public class EditEmployeeService: IEditEmployee
    {
        private readonly DbemployeeContext _context;
        public EditEmployeeService(DbemployeeContext context)
        {
            _context = context;
        }
        public async Task<Employee?> EditEmployee(int id,Employee employee)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existingEmployee == null)
            {
               return null;
            }
            if (employee.Name == null)
            {
                employee.Name = existingEmployee.Name;
              
            }
            if (employee.Salary == 0)
            {
                employee.Salary = existingEmployee.Salary;
            }
           
                existingEmployee.Name = employee.Name;
                existingEmployee.Salary = employee.Salary;
            
            await _context.SaveChangesAsync();
            return existingEmployee;
        }
    }
}
