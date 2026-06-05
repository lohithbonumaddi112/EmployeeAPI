using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Dto_s
{
    public enum UserRole
    {
        Root = 1,
        Admin = 2,
        User = 3

    }
    public class RegisterUserDTO
    {
      
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$",
    ErrorMessage = "Password must contain letters and numbers.")]

        public string Password { get; set; } =string.Empty;
        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }


    }
}
