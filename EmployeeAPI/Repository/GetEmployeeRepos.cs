using EmployeeAPI.Interfaces.IRepositories;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Repository
{
    public class GetEmployeeRepos: IGetEmployeeRepos
    {
        private readonly DbemployeeContext _context;
        
        public GetEmployeeRepos(DbemployeeContext context)
        {
            _context = context;

        }
        public async Task<List<Employee>> GetAll() { 
        
            var emp= await _context.Employees.ToListAsync();
            return emp;
        }
        public async Task<Employee> GetById(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            return emp;
        }

    }
}
