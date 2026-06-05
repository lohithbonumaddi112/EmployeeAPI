using EmployeeAPI.Dto_s;
using EmployeeAPI.Interfaces;
using EmployeeAPI.Middleware;
using EmployeeAPI.Models;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static EmployeeAPI.Dto_s.loginDTO;
using EmployeeAPI.Filters;
using Asp.Versioning;
using EmployeeAPI.Interfaces.IServices;


namespace EmployeeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DbemployeeContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(DbemployeeContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(loginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.username);
            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", loginDto.username);
                return NotFound("User not Found");
            }
            bool IsValid = BCrypt.Net.BCrypt.Verify(loginDto.password, user.Password);
            if (!IsValid) {
                return Unauthorized("Invalid password");
}


            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            _logger.LogInformation("User {Username} logged in successfully", loginDto.username);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler()
                    .WriteToken(token)
            });
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly IRegisterUser _registerUserService;
        private readonly ILogger<UserController> _logger;
        public UserController(IRegisterUser registerUserService, ILogger<UserController> logger)
        {
            _registerUserService = registerUserService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            
            var user = await _registerUserService.RegisterUser(registerUserDTO);
            if (user == null) { 
                return BadRequest("Username already exists");
            }
            
          
            _logger.LogInformation("Registered new user with username: {Username}", registerUserDTO.Username);
            return Ok(user);
        }
    }
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IGetEmployee _employeeService;
        private readonly ICreateEmployee _createEmployeeService;
        private readonly IEditEmployee _editEmployeeService;
        private readonly IDeleteEmployee _deleteEmployeeService;
        public EmployeeController(IGetEmployee employeeService, ICreateEmployee createEmployeeService, IEditEmployee editEmployee, IDeleteEmployee deleteEmployee, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _createEmployeeService = createEmployeeService;
            _editEmployeeService = editEmployee;
            _deleteEmployeeService = deleteEmployee;
            _logger = logger;
        }
        [HttpGet("cache-test")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60)]
        public IActionResult CacheTest()
        {
            _logger.LogInformation("Cache Test Executed");
            return Ok(DateTime.Now);
        }
        [HttpGet("version-test")]
        public IActionResult GetV1()
        {
            return Ok(new
            {
                Message = "This is Version 1",
                Version = "1.0"
            });
        }
        [HttpGet("test-error")]
        [ServiceFilter(typeof(CustExceptionFilter))]
        public IActionResult TestError()
        {
            _logger.LogWarning("TestError endpoint hit, about to throw an exception.");
            throw new Exception("Test exception");
        }

        [ServiceFilter(typeof(ResourceFilter))]
        [HttpGet("resource-test")]
        public IActionResult ResourceTest()
        {
            _logger.LogInformation("ResourceTest endpoint hit.");

            return Ok("Resource Filter Working");
        }
        [ServiceFilter(typeof(ResultFilter))]
        [HttpGet("result-test")]
        public IActionResult ResultTest()
        {
            _logger.LogInformation("ResultTest endpoint hit.");
            return Ok("Result Filter Working");
        }

        [ServiceFilter(typeof(ActionFilter))]
        [ServiceFilter(typeof(CustomAuth))]
        [HttpGet]
        [ResponseCache(Duration =60)]
        public async Task<IActionResult> Index()
        {
            var emp = await _employeeService.GetEmployees();
            var res = emp.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Salary = e.Salary,
                             

            }).ToList();

            _logger.LogInformation("Fetched {Count} employees", res.Count);
            return Ok(res);

        }
        [ServiceFilter(typeof(CustomAuth))]
        [HttpPost]

        public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto)
        {
           
            var emp = new Employee()
            {
                Name = dto.Name,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentID
            };
            var createdEmployee = await _createEmployeeService.CreateEmploye(emp);
            _logger.LogInformation("Created employee with ID {Id}", createdEmployee.Id);

            return Ok(createdEmployee);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployee(int id, UpdateEmployeeDto employee)
        {
            var emp = new Employee()
            {

                Name = employee.Name,
                Salary = employee.Salary
            };
            var updatedEmployee = await _editEmployeeService.EditEmployee(id, emp);
            if (updatedEmployee == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Updated employee with ID {Id}", id);
            return Ok(updatedEmployee);

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {

            var deletedEmployee = await _deleteEmployeeService.DeleteEmployees(id);
            if (deletedEmployee == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Deleted employee with ID {Id}", id);
            return Ok(deletedEmployee);

        }

        [HttpGet("salary-range/{minSalary}/{maxSalary}")]
        public async Task<IActionResult> SalaryRange(decimal minSalary, decimal maxSalary)
        {
            var res = await _employeeService.SalaryByRange(minSalary, maxSalary);
            if (!res.Any())
            {
                _logger.LogWarning(
                    "No employees found with salary between {MinSalary} and {MaxSalary}",
                    minSalary, maxSalary);

                return Ok("No Records found");
            }

            _logger.LogInformation("Fetched {Count} employees with salary between {MinSalary} and {MaxSalary}", res.Count, minSalary, maxSalary);


            return Ok(res);
        }
        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetEmployeeByName(string name)
        {
            var res = await _employeeService.GetEmployeesByName(name);
            if (res == null)
            {
                _logger.LogWarning("No employee found with name containing: {Name}", name);
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched employee with name containing: {Name}", name);
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> EmployeeGetByID(int id)
        {
            var re = await _employeeService.GetEmployeById(id);
            if (re == null)
            {
                _logger.LogWarning("No employee found with ID: {Id}", id);
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched employee with ID: {Id}", id);

            return Ok(re);
        }
        [HttpGet("all-salary-positive")]
        public async Task<IActionResult> AreAllEmployeesSalaryPositive()
        {
            var result = await _employeeService.CheckSalary();

            _logger.LogInformation("Checked whether all employees have positive salary. Result: {Result}", result);

            return Ok(result);
        }
        [HttpGet("count-salary-above/{minSalary}")]
        public async Task<IActionResult> CountEmployeesWithSalaryAbove(decimal minSalary)
        {
            var count = await _employeeService.CheckMinSalary(minSalary);
            _logger.LogInformation("Counted employees with salary above {MinSalary}. Count: {Count}", minSalary, count);
            return Ok(count);
        }
        [HttpGet("sort-ID")]
        public async Task<IActionResult> SortEmployeesById()
        {
            var sortedEmployees = await _employeeService.SortEmployeesBySalary();
            if (!sortedEmployees.Any())
            {
                _logger.LogWarning("No employees found to sort by ID");
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched {Count} employees sorted by ID", sortedEmployees.Count);
            return Ok(sortedEmployees);
        }
        [HttpGet("with-department")]
        public async Task<IActionResult> GetEmployeesWithDepartment()
        {
            var employeesWithDepartments = await _employeeService.GetEmployeesWithDepartment();
            if (!employeesWithDepartments.Any())
            {
                _logger.LogWarning("No employees found with department information");
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched {Count} employees with department information", employeesWithDepartments.Count);
            return Ok(employeesWithDepartments);
        }
        [HttpGet("department")]
        public async Task<IActionResult> GetDepartmentsWithEmployees()
        {
            var departmentsWithEmployees = await _employeeService.GetDepartmentsWithEmployees();
            if (!departmentsWithEmployees.Any())
            {
                _logger.LogWarning("No departments found with employee information");
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched {Count} departments with employee information", departmentsWithEmployees.Count);
            return Ok(departmentsWithEmployees);
        }
        [HttpGet("DPTS-Names")]
        public async Task<IActionResult> GetDptsWithEmployees()
        {
            var departmentsWithEmployees = await _employeeService.GetDptsWithEmployee();
            if (!departmentsWithEmployees.Any())
            {
                _logger.LogWarning("No departments found with employee information");
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched {Count} departments with employee information", departmentsWithEmployees.Count);
            return Ok(departmentsWithEmployees);
        }
        [HttpGet("GetBySPSQl-ID/{id}")]
        public async Task<IActionResult> GetEmployeeByIdUsingSP(int id)
        {
            var employee = await _employeeService.GetEmployeeDepartmentById(id);
            if (!employee.Any())
            {
                _logger.LogWarning("No employee found with ID: {Id} using stored procedure", id);
                return Ok("No Records found");
            }
            _logger.LogInformation("Fetched employee with ID: {Id} using stored procedure", id);
            return Ok(employee);
        }


    }
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeeV2Controller : ControllerBase
    {

        [HttpGet]
        public IActionResult GetV2()
        {
            return Ok(new
            {
                Message = "This is Version 2",
                Version = "2.0"
            });
        }
    }
}