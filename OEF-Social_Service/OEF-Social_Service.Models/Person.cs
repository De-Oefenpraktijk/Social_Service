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
        public Guid Id { get; set; } = Guid.NewGuid();
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
        public string Institution { get; set; } = String.Empty;
        public string Theme { get; set; } = String.Empty;
        public string ResidencePlace { get; set; } = String.Empty;
    }
}
