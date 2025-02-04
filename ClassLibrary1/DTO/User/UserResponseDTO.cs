using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTO.User
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int RoleId { get; set; }

        public bool? IsStudent { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
