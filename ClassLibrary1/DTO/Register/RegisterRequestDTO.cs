using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTO.Register
{
    public class RegisterRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }     
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public bool? IsStudent { get; set; }
        public string StudentCode { get; set; }
        public string University { get; set; }
        public string StudentCardImage { get; set; }  

    }
}
