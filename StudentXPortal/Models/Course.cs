namespace StudentX.StudentXPortal.Models
{

    public class Course
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}