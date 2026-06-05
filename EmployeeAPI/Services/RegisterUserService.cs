using System;
using EmployeeAPI.Dto_s;
using EmployeeAPI.Models;
using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EmployeeAPI.Services
{
    public class RegisterUserService: IRegisterUser
    {
        public readonly DbemployeeContext _context;

        public RegisterUserService(DbemployeeContext context)
        {
            _context = context;
        }
        public async Task<Users?> RegisterUser(RegisterUserDTO registerUserDTO)
        {
          
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == registerUserDTO.Username);
            if (existingUser != null)
            {
               return null;
            }
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);
            var user = new Users
            {
                UserName = registerUserDTO.Username,
                Password = hashPassword,
                Role = registerUserDTO.Role.ToString(),
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
