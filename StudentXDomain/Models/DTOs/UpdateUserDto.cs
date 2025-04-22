using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentX.StudentXDomain.Models;

namespace StudentX.StudentXDomain.Models.DTOs
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType UserType { get; set; }
    }
}