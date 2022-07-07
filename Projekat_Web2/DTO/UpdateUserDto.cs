using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.DTO
{
    public class UpdateUserDto
    {
        public string Username { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public string Address { get; set; }

        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
