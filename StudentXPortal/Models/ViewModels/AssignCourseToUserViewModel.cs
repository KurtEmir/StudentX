using System.Collections.Generic;

namespace StudentX.StudentXPortal.Models.ViewModels
{
    public class AssignCourseToUserViewModel
    {
        public int SelectedUserId { get; set; }
        public int SelectedCourseId { get; set; }
        public List<User> Users { get; set; }
        public List<Course> Courses { get; set; }
    }
}
