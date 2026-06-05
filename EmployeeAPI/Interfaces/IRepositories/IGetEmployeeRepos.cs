using EmployeeAPI.Models;

namespace EmployeeAPI.Interfaces.IRepositories
{
    public interface IGetEmployeeRepos
    {
        public Task<List<Employee>> GetAll();
        public Task<Employee> GetById(int id);
    }
}
