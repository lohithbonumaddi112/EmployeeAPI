using EmployeeAPI.Interfaces;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Services
{
    public class DeleteEmployeeService : IDeleteEmployee
    {
        private readonly DbemployeeContext _context;
        public DeleteEmployeeService(DbemployeeContext context)
        {
            _context = context;
        }
        public async Task<Employee?> DeleteEmployees(int id)
        { 
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                return null;
            }

            _context.Employees.Remove(existingEmployee);
            await _context.SaveChangesAsync();
            return existingEmployee;
        }
    }
}
