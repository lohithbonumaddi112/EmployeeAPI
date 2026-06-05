using System;
using EmployeeAPI.Dto_s;
using EmployeeAPI.Models;
namespace EmployeeAPI.Interfaces
{
    public interface IRegisterUser
    {
        Task<Users?> RegisterUser(RegisterUserDTO registerUserDTO);
    }
}
