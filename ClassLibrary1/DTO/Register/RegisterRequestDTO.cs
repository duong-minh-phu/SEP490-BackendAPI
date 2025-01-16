using System;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary1.DTO.Register
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Role ID is required.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "IsStudent is required.")]
        public bool? IsStudent { get; set; }

        [Required(ErrorMessage = "Student Code is required.")]
        public string StudentCode { get; set; }

        [Required(ErrorMessage = "University is required.")]
        public string University { get; set; }

        [Required(ErrorMessage = "Student Card Image is required.")]
        public string StudentCardImage { get; set; }
    }
}
