using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEF_Social_Service.Models
{
    public class Person
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = String.Empty;
        [Required]
        public string LastName { get; set; } = String.Empty;
        [Required]
        public string Username { get; set; } = String.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = String.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
        [Required]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        [Required]
        public string Role { get; set; } = "User";
        public List<string>? Educations { get; set; }
        public List<string>? Specializations { get; set; }
        public string ResidencePlace { get; set; } = String.Empty;
    }
}
