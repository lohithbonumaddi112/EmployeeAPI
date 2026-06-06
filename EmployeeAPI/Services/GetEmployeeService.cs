using System;
using EmployeeAPI.Dto_s;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Interfaces.IRepositories;
using EmployeeAPI.Models;
using EmployeeAPI.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Services
{
    public class GetEmployeeService : IGetEmployee
    {
        private readonly ILogger<GetEmployeeService> _logger;
        private readonly DbemployeeContext _context;
        private readonly IGetEmployeeRepos _GetEmployeeRepos;
        
        public GetEmployeeService(DbemployeeContext context, ILogger<GetEmployeeService> logger,IGetEmployeeRepos getEmployee)
        {
            _context = context;
            _logger = logger;
            _GetEmployeeRepos = getEmployee;
        
        }
        public async Task<List<Employee>> GetEmployees()
        {
            var emp = await _GetEmployeeRepos.GetAll();
           _logger.LogInformation("GetEmployees service executed");
            return  emp;
        }
        public async Task<List<EmployeeDto>> SalaryByRange(decimal minSalary, decimal maxSalary)
        {
            var employees = await _context.Employees
                .Where(e => e.Salary >= minSalary && e.Salary <= maxSalary)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Salary = e.Salary
                })
                .ToListAsync();
            
            return employees;
        }
        public async Task<EmployeeDto?> GetEmployeesByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var emp = await _context.Employees
                 .Where(e => e.Name != null && e.Name.Equals(name))
                 .Select(e => new EmployeeDto
                 {
                     Id = e.Id,
                     Name = e.Name,
                     Salary = e.Salary
                 })
                 .FirstOrDefaultAsync();
            return emp;

        }
        public async Task<EmployeeDto?> GetEmployeById(int Id)
        {
            if (Id == 0) { return null; }
            var em = await _GetEmployeeRepos.GetById(Id);
            if (em == null) { return null; }
            return new EmployeeDto
            {
                Id = em.Id,
                Name = em.Name,
                Salary = em.Salary
            };
        }
        public async Task<bool> CheckSalary()
        {

            var check = await _context.Employees.AnyAsync(e => e.Salary > 0);
            return check;
        }
        public async Task<int> CheckMinSalary(decimal minSalary)
        {
            var count = await _context.Employees.CountAsync(e => e.Salary >= minSalary);
            return count;
        }
        public async Task<List<EmployeeDto>> SortEmployeesBySalary()
        {
            var sortedEmployees = await _context.Employees
                .OrderBy(e => e.Id)
                .ThenBy(e => e.Salary)
                .Skip(1)
                .Take(3)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Salary = e.Salary
                })
                .ToListAsync();
            return sortedEmployees;

        }
        public async Task<List<EmployeeDepartmentDto>> GetEmployeesWithDepartment()
        {

            var employeesWithDepartments = await _context.Employees.OrderBy(e => e.Id)
                .Join(_context.Department,
                e => e.DepartmentId,
                d => d.Id,
                (e, d) => new { Employee = e, Department = d })
                .Select(e => new EmployeeDepartmentDto
                {
                    Id = e.Employee.Id,
                    Name = e.Employee.Name,
                    Salary = e.Employee.Salary,
                    DepartmentName = e.Department != null ? e.Department.DepartmentName : null
                })
                .ToListAsync();
            return employeesWithDepartments;
        }
        public async Task<List<DepartmentEmployeesDto>> GetDepartmentsWithEmployees()
        {
            var emp = await _context.Department
                .GroupJoin(_context.Employees, e => e.Id, d => d.DepartmentId, (e, d) => new { Department = e, Employees = d })
                .Select(e => new DepartmentEmployeesDto {
                    DepartmentName = e.Department.DepartmentName,
                    Employees = e.Employees.Select(emp => new EmployeeDto
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Salary = emp.Salary
                    }).ToList()
                }).ToListAsync();
            return emp;
        }
        public async Task<List<EmployeeDepartmentDto>> GetDptsWithEmployee()
        {
            var emp = await _context.Employees
                           .Include(e => e.Department)
                        .Select(e => new EmployeeDepartmentDto
                        { Id = e.Id,
                            Name = e.Name,
                            Salary = e.Salary,
                            DepartmentName = e.Department != null ? e.Department!.DepartmentName : null }).ToListAsync();
            return emp;
        }
        public async Task<List<EmployeeDepartmentDto>> GetEmployeeDepartmentById(int id)
        {
        var res=await _context.EmployeeDepartmentDtos
        .FromSqlInterpolated($"EXEC GetEmployeeDepartmentById {id}")
        .ToListAsync();
            return res;
        }

    }
}
