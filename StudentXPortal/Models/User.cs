using System;
using StudentX.StudentXPortal.Models;

namespace StudentX.StudentXPortal.Models
{

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    }


}