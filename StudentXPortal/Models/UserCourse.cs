using StudentX.StudentXPortal.Models;

namespace StudentX.StudentXPortal.Models
{
    public class UserCourse
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Course Course { get; set; }


        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }



    }
}