using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace EmployeeAPI.Dto_s
{
    public class CreateEmployeeDto
    {

        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z ]+$",
            ErrorMessage = "Name should contain only letters.")]
        public string? Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage="ID should be 1 to 3")]
        public int DepartmentID { get; set; }

    }
}
